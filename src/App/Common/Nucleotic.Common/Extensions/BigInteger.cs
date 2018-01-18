using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Org.BouncyCastle.Security;

namespace Nucleotic.Common.Extensions
{
    public class BigInteger
    {

        private int _sign; // -1 means -ve; +1 means +ve; 0 means 0;
        private int[] _magnitude; // array of ints with [0] being the most significant
        private int _nBits = -1; // cache bitCount() value
        private int _nBitLength = -1; // cache BitLength() value
        private const long IMask = 0xffffffffL;
        private long _mQuote = -1L; // -m^(-1) Mod b, b = 2^32 (see Montgomery mult.)

        private BigInteger()
        {
        }

        private BigInteger(int nWords)
        {
            _sign = 1;
            _magnitude = new int[nWords];
        }

        private BigInteger(int signum, int[] mag)
        {
            _sign = signum;
            if (mag.Length > 0)
            {
                int i = 0;
                while (i < mag.Length && mag[i] == 0)
                {
                    i++;
                }
                if (i == 0)
                {
                    _magnitude = mag;
                }
                else
                {
                    // strip leading 0 bytes
                    var newMag = new int[mag.Length - i];
                    Array.Copy(mag, i, newMag, 0, newMag.Length);
                    _magnitude = newMag;
                    if (newMag.Length == 0)
                        _sign = 0;
                }
            }
            else
            {
                _magnitude = mag;
                _sign = 0;
            }
        }

        public BigInteger(string sval) //throws FormatException
            : this(sval, 10) { }

        public BigInteger(string sval, int rdx) //throws FormatException
        {
            if (sval.Length == 0)
            {
                throw new FormatException("Zero length BigInteger");
            }

            NumberStyles style;
            switch (rdx)
            {
                case 10:
                    style = NumberStyles.Integer;
                    break;
                case 16:
                    style = NumberStyles.AllowHexSpecifier;
                    break;
                default:
                    throw new FormatException("Only base 10 or 16 alllowed");
            }


            int index = 0;
            _sign = 1;

            if (sval[0] == '-')
            {
                if (sval.Length == 1)
                {
                    throw new FormatException("Zero length BigInteger");
                }

                _sign = -1;
                index = 1;
            }

            // strip leading zeros from the string value
            while (index < sval.Length && int.Parse(sval[index].ToString(), style) == 0)
            {
                index++;
            }

            if (index >= sval.Length)
            {
                // zero value - we're done
                _sign = 0;
                _magnitude = new int[0];
                return;
            }

            //////
            // could we work out the Max number of ints required to store
            // sval.length digits in the given base, then allocate that
            // storage in one hit?, then generate the magnitude in one hit too?
            //////

            var b = Zero;
            var r = ValueOf(rdx);
            while (index < sval.Length)
            {
                // (optimise this by taking chunks of digits instead?)
                b = b.multiply(r).add(ValueOf(Int32.Parse(sval[index].ToString(), style)));
                index++;
            }

            _magnitude = b._magnitude;
            return;
        }

        public BigInteger(byte[] bval) //throws FormatException
        {
            if (bval.Length == 0)
            {
                throw new FormatException("Zero length BigInteger");
            }

            _sign = 1;
            if (bval[0] < 0)
            {
                // FIXME:
                int iBval;
                _sign = -1;
                // strip leading sign bytes
                for (iBval = 0; iBval < bval.Length && ((sbyte)bval[iBval] == -1); iBval++) ;
                _magnitude = new int[(bval.Length - iBval) / 2 + 1];
                // copy bytes to magnitude
                // invert bytes then add one to find magnitude of value
            }
            else
            {
                // strip leading zero bytes and return magnitude bytes
                _magnitude = MakeMagnitude(bval);
            }
        }

        private static int[] MakeMagnitude(IReadOnlyList<byte> bval)
        {
            int i;
            int firstSignificant;

            // strip leading zeros
            for (firstSignificant = 0; firstSignificant < bval.Count
                    && bval[firstSignificant] == 0; firstSignificant++) ;

            if (firstSignificant >= bval.Count)
            {
                return new int[0];
            }

            var nInts = (bval.Count - firstSignificant + 3) / 4;
            var bCount = (bval.Count - firstSignificant) % 4;
            if (bCount == 0)
                bCount = 4;

            var mag = new int[nInts];
            var v = 0;
            var magnitudeIndex = 0;
            for (i = firstSignificant; i < bval.Count; i++)
            {
                v <<= 8;
                v |= bval[i] & 0xff;
                bCount--;
                if (bCount <= 0)
                {
                    mag[magnitudeIndex] = v;
                    magnitudeIndex++;
                    bCount = 4;
                    v = 0;
                }
            }

            if (magnitudeIndex < mag.Length)
            {
                mag[magnitudeIndex] = v;
            }

            return mag;
        }

        public BigInteger(int sign, byte[] mag) //throws FormatException
        {
            if (sign < -1 || sign > 1)
            {
                throw new FormatException("Invalid sign value");
            }

            if (sign == 0)
            {
                this._sign = 0;
                this._magnitude = new int[0];
                return;
            }

            // copy bytes
            this._magnitude = MakeMagnitude(mag);
            this._sign = sign;
        }

        public BigInteger(int numBits, Random rnd) //throws ArgumentException
        {
            if (numBits < 0)
            {
                throw new ArgumentException("numBits must be non-negative");
            }

            int nBytes = (numBits + 7) / 8;

            byte[] b = new byte[nBytes];

            if (nBytes > 0)
            {
                nextRndBytes(rnd, b);
                // strip off any excess bits in the MSB
                b[0] &= rndMask[8 * nBytes - numBits];
            }

            this._magnitude = MakeMagnitude(b);
            this._sign = 1;
            this._nBits = -1;
            this._nBitLength = -1;
        }

        private static readonly int BITS_PER_BYTE = 8;
        private static readonly int BYTES_PER_INT = 4;

        /**
         * strictly speaking this is a little dodgey from a compliance
         * point of view as it forces people to be using SecureRandom as
         * well, that being said - this implementation is for a crypto
         * library and you do have the source!
         */
        private void nextRndBytes(Random rnd, byte[] bytes)
        {
            var numRequested = bytes.Length;
            int numGot = 0,
            r = 0;

            var random = rnd as SecureRandom;
            if (random != null)
            {
                random.NextBytes(bytes);
            }
            else
            {
                for (;;)
                {
                    for (int i = 0; i < BYTES_PER_INT; i++)
                    {
                        if (numGot == numRequested)
                        {
                            return;
                        }

                        r = (i == 0 ? rnd.Next() : r >> BITS_PER_BYTE);
                        bytes[numGot++] = (byte)r;
                    }
                }
            }
        }

        private static readonly byte[] rndMask = { (byte)255, 127, 63, 31, 15, 7, 3, 1 };

        public BigInteger(int bitLength, int certainty, Random rnd) //throws ArithmeticException
        {
            int nBytes = (bitLength + 7) / 8;

            byte[] b = new byte[nBytes];

            do
            {
                if (nBytes > 0)
                {
                    nextRndBytes(rnd, b);
                    // strip off any excess bits in the MSB
                    b[0] &= rndMask[8 * nBytes - bitLength];
                }

                this._magnitude = MakeMagnitude(b);
                this._sign = 1;
                this._nBits = -1;
                this._nBitLength = -1;
                this._mQuote = -1L;

                if (certainty > 0 && bitLength > 2)
                {
                    this._magnitude[this._magnitude.Length - 1] |= 1;
                }
            } while (this.BitLength() != bitLength || !this.IsProbablePrime(certainty));
        }

        public BigInteger abs()
        {
            return (_sign >= 0) ? this : this.negate();
        }

        /**
         * return a = a + b - b preserved.
         */
        private int[] add(int[] a, int[] b)
        {
            int tI = a.Length - 1;
            int vI = b.Length - 1;
            long m = 0;

            while (vI >= 0)
            {
                m += (((long)a[tI]) & IMask) + (((long)b[vI--]) & IMask);
                a[tI--] = (int)m;
                m = (long)((ulong)m >> 32);
            }

            while (tI >= 0 && m != 0)
            {
                m += (((long)a[tI]) & IMask);
                a[tI--] = (int)m;
                m = (long)((ulong)m >> 32);
            }

            return a;
        }

        public BigInteger add(BigInteger val) //throws ArithmeticException
        {
            if (val._sign == 0 || val._magnitude.Length == 0)
                return this;
            if (this._sign == 0 || this._magnitude.Length == 0)
                return val;

            if (val._sign < 0)
            {
                if (this._sign > 0)
                    return this.Subtract(val.negate());
            }
            else
            {
                if (this._sign < 0)
                    return val.Subtract(this.negate());
            }

            // both BigIntegers are either +ve or -ve; set the sign later

            int[] mag,
            op;

            if (this._magnitude.Length < val._magnitude.Length)
            {
                mag = new int[val._magnitude.Length + 1];

                Array.Copy(val._magnitude, 0, mag, 1, val._magnitude.Length);
                op = this._magnitude;
            }
            else
            {
                mag = new int[this._magnitude.Length + 1];

                Array.Copy(this._magnitude, 0, mag, 1, this._magnitude.Length);
                op = val._magnitude;
            }

            return new BigInteger(this._sign, add(mag, op));
        }

        public int BitCount()
        {
            if (_nBits != -1) return _nBits;
            _nBits = 0;
            foreach (var t in _magnitude)
            {
                _nBits += BitCounts[t & 0xff];
                _nBits += BitCounts[(t >> 8) & 0xff];
                _nBits += BitCounts[(t >> 16) & 0xff];
                _nBits += BitCounts[(t >> 24) & 0xff];
            }

            return _nBits;
        }

        private static readonly byte[] BitCounts = {0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4, 1,
            2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4,
            4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3,
            4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5,
            3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 1, 2, 2, 3, 2,
            3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3,
            3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6,
            7, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6,
            5, 6, 6, 7, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 4, 5, 5, 6, 5, 6, 6, 7, 5,
            6, 6, 7, 6, 7, 7, 8};

        private int BitLength(int indx, int[] mag)
        {
            int bitLength;

            if (mag.Length == 0)
            {
                return 0;
            }
            else
            {
                while (indx != mag.Length && mag[indx] == 0)
                {
                    indx++;
                }

                if (indx == mag.Length)
                {
                    return 0;
                }

                // bit length for everything after the first int
                bitLength = 32 * ((mag.Length - indx) - 1);

                // and determine bitlength of first int
                bitLength += BitLen(mag[indx]);

                if (_sign < 0)
                {
                    // Check if magnitude is a power of two
                    bool pow2 = ((BitCounts[mag[indx] & 0xff])
                            + (BitCounts[(mag[indx] >> 8) & 0xff])
                            + (BitCounts[(mag[indx] >> 16) & 0xff]) + (BitCounts[(mag[indx] >> 24) & 0xff])) == 1;

                    for (int i = indx + 1; i < mag.Length && pow2; i++)
                    {
                        pow2 = (mag[i] == 0);
                    }

                    bitLength -= (pow2 ? 1 : 0);
                }
            }

            return bitLength;
        }

        public int BitLength()
        {
            if (_nBitLength == -1)
            {
                _nBitLength = _sign == 0 ? 0 : BitLength(0, _magnitude);
            }

            return _nBitLength;
        }

        //
        // bitLen(val) is the number of bits in val.
        //
        private static int BitLen(int w)
        {
            // Binary search - decision tree (5 tests, rarely 6)
            return (w < 1 << 15 ? (w < 1 << 7
                    ? (w < 1 << 3 ? (w < 1 << 1
                            ? (w < 1 << 0 ? (w < 0 ? 32 : 0) : 1)
                            : (w < 1 << 2 ? 2 : 3)) : (w < 1 << 5
                            ? (w < 1 << 4 ? 4 : 5)
                            : (w < 1 << 6 ? 6 : 7)))
                    : (w < 1 << 11
                            ? (w < 1 << 9 ? (w < 1 << 8 ? 8 : 9) : (w < 1 << 10 ? 10 : 11))
                            : (w < 1 << 13 ? (w < 1 << 12 ? 12 : 13) : (w < 1 << 14 ? 14 : 15)))) : (w < 1 << 23 ? (w < 1 << 19
                    ? (w < 1 << 17 ? (w < 1 << 16 ? 16 : 17) : (w < 1 << 18 ? 18 : 19))
                    : (w < 1 << 21 ? (w < 1 << 20 ? 20 : 21) : (w < 1 << 22 ? 22 : 23))) : (w < 1 << 27
                    ? (w < 1 << 25 ? (w < 1 << 24 ? 24 : 25) : (w < 1 << 26 ? 26 : 27))
                    : (w < 1 << 29 ? (w < 1 << 28 ? 28 : 29) : (w < 1 << 30 ? 30 : 31)))));
        }

        private static readonly byte[] BitLengths = {0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8};

        public int CompareTo(object o)
        {
            return CompareTo((BigInteger)o);
        }

        /**
         * unsigned comparison on two arrays - note the arrays may
         * start with leading zeros.
         */
        private static int CompareTo(int xIndx, int[] x, int yIndx, int[] y)
        {
            while (xIndx != x.Length && x[xIndx] == 0)
            {
                xIndx++;
            }

            while (yIndx != y.Length && y[yIndx] == 0)
            {
                yIndx++;
            }

            if ((x.Length - xIndx) < (y.Length - yIndx))
            {
                return -1;
            }

            if ((x.Length - xIndx) > (y.Length - yIndx))
            {
                return 1;
            }

            // lengths of magnitudes the same, test the magnitude values

            while (xIndx < x.Length)
            {
                long v1 = (long)(x[xIndx++]) & IMask;
                long v2 = (long)(y[yIndx++]) & IMask;
                if (v1 < v2)
                {
                    return -1;
                }
                if (v1 > v2)
                {
                    return 1;
                }
            }

            return 0;
        }

        public int CompareTo(BigInteger val)
        {
            if (_sign < val._sign)
                return -1;
            if (_sign > val._sign)
                return 1;

            return CompareTo(0, _magnitude, 0, val._magnitude);
        }

        /**
         * return z = x / y - done in place (z value preserved, x contains the
         * remainder)
         */
        private int[] Divide(int[] x, int[] y)
        {
            int xyCmp = CompareTo(0, x, 0, y);
            int[] count;

            if (xyCmp > 0)
            {
                int[] c;

                int shift = BitLength(0, x) - BitLength(0, y);

                if (shift > 1)
                {
                    c = shiftLeft(y, shift - 1);
                    count = shiftLeft(One._magnitude, shift - 1);
                    if (shift % 32 == 0)
                    {
                        // Special case where the shift is the size of an int.
                        int[] countSpecial = new int[shift / 32 + 1];
                        Array.Copy(count, 0, countSpecial, 1, countSpecial.Length - 1);
                        countSpecial[0] = 0;
                        count = countSpecial;
                    }
                }
                else
                {
                    c = new int[x.Length];
                    count = new int[1];

                    Array.Copy(y, 0, c, c.Length - y.Length, y.Length);
                    count[0] = 1;
                }

                int[] iCount = new int[count.Length];

                Subtract(0, x, 0, c);
                Array.Copy(count, 0, iCount, 0, count.Length);

                int xStart = 0;
                int cStart = 0;
                int iCountStart = 0;

                for (;;)
                {
                    int cmp = CompareTo(xStart, x, cStart, c);

                    while (cmp >= 0)
                    {
                        Subtract(xStart, x, cStart, c);
                        add(count, iCount);
                        cmp = CompareTo(xStart, x, cStart, c);
                    }

                    xyCmp = CompareTo(xStart, x, 0, y);

                    if (xyCmp > 0)
                    {
                        if (x[xStart] == 0)
                        {
                            xStart++;
                        }

                        shift = BitLength(cStart, c) - BitLength(xStart, x);

                        if (shift == 0)
                        {
                            c = shiftRightOne(cStart, c);
                            iCount = shiftRightOne(iCountStart, iCount);
                        }
                        else
                        {
                            c = shiftRight(cStart, c, shift);
                            iCount = shiftRight(iCountStart, iCount, shift);
                        }

                        if (c[cStart] == 0)
                        {
                            cStart++;
                        }

                        if (iCount[iCountStart] == 0)
                        {
                            iCountStart++;
                        }
                    }
                    else if (xyCmp == 0)
                    {
                        add(count, One._magnitude);
                        for (int i = xStart; i != x.Length; i++)
                        {
                            x[i] = 0;
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (xyCmp == 0)
            {
                count = new int[1];

                count[0] = 1;
            }
            else
            {
                count = new int[1];

                count[0] = 0;
            }

            return count;
        }

        public BigInteger Divide(BigInteger val) //throws ArithmeticException
        {
            if (val._sign == 0)
            {
                throw new ArithmeticException("Divide by zero");
            }

            if (_sign == 0)
            {
                return BigInteger.Zero;
            }

            if (val.CompareTo(BigInteger.One) == 0)
            {
                return this;
            }

            int[] mag = new int[this._magnitude.Length];
            Array.Copy(this._magnitude, 0, mag, 0, mag.Length);

            return new BigInteger(this._sign * val._sign, Divide(mag, val._magnitude));
        }

        public BigInteger[] divideAndRemainder(BigInteger val) //throws ArithmeticException
        {
            if (val._sign == 0)
            {
                throw new ArithmeticException("Divide by zero");
            }

            BigInteger[] biggies = new BigInteger[2];

            if (_sign == 0)
            {
                biggies[0] = biggies[1] = BigInteger.Zero;

                return biggies;
            }

            if (val.CompareTo(BigInteger.One) == 0)
            {
                biggies[0] = this;
                biggies[1] = BigInteger.Zero;

                return biggies;
            }

            int[] remainder = new int[this._magnitude.Length];
            Array.Copy(this._magnitude, 0, remainder, 0, remainder.Length);

            int[] quotient = Divide(remainder, val._magnitude);

            biggies[0] = new BigInteger(this._sign * val._sign, quotient);
            biggies[1] = new BigInteger(this._sign, remainder);

            return biggies;
        }

        public override bool Equals(Object val)
        {
            if (val == this)
                return true;

            if (!typeof(BigInteger).IsInstanceOfType(val))
                return false;
            BigInteger biggie = (BigInteger)val;

            if (biggie._sign != _sign || biggie._magnitude.Length != _magnitude.Length)
                return false;

            for (int i = 0; i < _magnitude.Length; i++)
            {
                if (biggie._magnitude[i] != _magnitude[i])
                    return false;
            }

            return true;
        }

        public BigInteger Gcd(BigInteger val)
        {
            if (val._sign == 0)
                return this.abs();
            else if (_sign == 0)
                return val.abs();

            var u = this;
            var v = val;

            while (v._sign != 0)
            {
                var r = u.Mod(v);
                u = v;
                v = r;
            }

            return u;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public int IntValue()
        {
            if (_magnitude.Length == 0)
            {
                return 0;
            }

            if (_sign < 0)
            {
                return -_magnitude[_magnitude.Length - 1];
            }
            return _magnitude[_magnitude.Length - 1];
        }

        /**
         * return whether or not a BigInteger is probably prime with a
         * probability of 1 - (1/2)**certainty.
         * <p>
         * From Knuth Vol 2, pg 395.
         */
        public bool IsProbablePrime(int certainty)
        {
            if (certainty == 0)
            {
                return true;
            }

            BigInteger n = this.abs();

            if (n.Equals(Two))
            {
                return true;
            }

            if (n.Equals(One) || !n.TestBit(0))
            {
                return false;
            }

            if ((certainty & 0x1) == 1)
            {
                certainty = certainty / 2 + 1;
            }
            else
            {
                certainty /= 2;
            }

            //
            // let n = 1 + 2^kq
            //
            BigInteger q = n.Subtract(One);
            int k = q.GetLowestSetBit();

            q = q.shiftRight(k);

            Random rnd = new Random();
            for (int i = 0; i <= certainty; i++)
            {
                BigInteger x;

                do
                {
                    x = new BigInteger(n.BitLength(), rnd);
                } while (x.CompareTo(One) <= 0 || x.CompareTo(n) >= 0);

                int j = 0;
                BigInteger y = x.ModPow(q, n);

                while (!((j == 0 && y.Equals(One)) || y.Equals(n.Subtract(One))))
                {
                    if (j > 0 && y.Equals(One))
                    {
                        return false;
                    }
                    if (++j == k)
                    {
                        return false;
                    }
                    y = y.ModPow(Two, n);
                }
            }

            return true;
        }

        public long LongValue()
        {
            long val = 0;

            if (_magnitude.Length == 0)
            {
                return 0;
            }

            if (_magnitude.Length > 1)
            {
                val = ((long)_magnitude[_magnitude.Length - 2] << 32)
                        | (_magnitude[_magnitude.Length - 1] & IMask);
            }
            else
            {
                val = (_magnitude[_magnitude.Length - 1] & IMask);
            }

            if (_sign < 0)
            {
                return -val;
            }
            else
            {
                return val;
            }
        }

        public BigInteger Max(BigInteger val)
        {
            return (CompareTo(val) > 0) ? this : val;
        }

        public BigInteger Min(BigInteger val)
        {
            return (CompareTo(val) < 0) ? this : val;
        }

        public BigInteger Mod(BigInteger m) //throws ArithmeticException
        {
            if (m._sign <= 0)
            {
                throw new ArithmeticException("BigInteger: modulus is not positive");
            }

            BigInteger biggie = this.remainder(m);

            return (biggie._sign >= 0 ? biggie : biggie.add(m));
        }

        public BigInteger ModInverse(BigInteger m) //throws ArithmeticException
        {
            if (m._sign != 1)
            {
                throw new ArithmeticException("Modulus must be positive");
            }

            BigInteger x = new BigInteger();
            BigInteger y = new BigInteger();

            BigInteger gcd = BigInteger.ExtEuclid(this, m, x, y);

            if (!gcd.Equals(BigInteger.One))
            {
                throw new ArithmeticException("Numbers not relatively prime.");
            }

            if (x.CompareTo(BigInteger.Zero) < 0)
            {
                x = x.add(m);
            }

            return x;
        }

        /**
         * Calculate the numbers u1, u2, and u3 such that:
         *
         * u1 * a + u2 * b = u3
         *
         * where u3 is the greatest common divider of a and b.
         * a and b using the extended Euclid algorithm (refer p. 323
         * of The Art of Computer Programming vol 2, 2nd ed).
         * This also seems to have the side effect of calculating
         * some form of multiplicative inverse.
         *
         * @param a    First number to calculate gcd for
         * @param b    Second number to calculate gcd for
         * @param u1Out      the return object for the u1 value
         * @param u2Out      the return object for the u2 value
         * @return     The greatest common divisor of a and b
         */
        private static BigInteger ExtEuclid(BigInteger a, BigInteger b, BigInteger u1Out,
                BigInteger u2Out)
        {
            BigInteger u1 = BigInteger.One;
            BigInteger u3 = a;
            BigInteger v1 = BigInteger.Zero;
            BigInteger v3 = b;

            while (v3.CompareTo(BigInteger.Zero) > 0)
            {
                BigInteger q,
                tn;
                //tv;

                q = u3.Divide(v3);

                tn = u1.Subtract(v1.multiply(q));
                u1 = v1;
                v1 = tn;

                tn = u3.Subtract(v3.multiply(q));
                u3 = v3;
                v3 = tn;
            }

            u1Out._sign = u1._sign;
            u1Out._magnitude = u1._magnitude;

            var res = u3.Subtract(u1.multiply(a)).Divide(b);
            u2Out._sign = res._sign;
            u2Out._magnitude = res._magnitude;

            return u3;
        }

        /**
         * zero out the array x
         */
        private void zero(int[] x)
        {
            for (int i = 0; i != x.Length; i++)
            {
                x[i] = 0;
            }
        }

        public BigInteger ModPow(
            BigInteger exponent,
            BigInteger m)
        //throws ArithmeticException
        {
            int[] zVal = null;
            int[] yAccum = null;

            // Montgomery exponentiation is only possible if the modulus is odd,
            // but AFAIK, this is always the case for crypto algo's
            bool useMonty = ((m._magnitude[m._magnitude.Length - 1] & 1) == 1);
            long mQ = 0;
            if (useMonty)
            {
                mQ = m.getMQuote();

                // tmp = this * R Mod m
                BigInteger tmp = this.shiftLeft(32 * m._magnitude.Length).Mod(m);
                zVal = tmp._magnitude;

                useMonty = (zVal.Length == m._magnitude.Length);

                if (useMonty)
                {
                    yAccum = new int[m._magnitude.Length + 1];
                }
            }

            if (!useMonty)
            {
                if (_magnitude.Length <= m._magnitude.Length)
                {
                    //zAccum = new int[m.magnitude.Length * 2];
                    zVal = new int[m._magnitude.Length];

                    Array.Copy(_magnitude, 0, zVal, zVal.Length - _magnitude.Length,
                            _magnitude.Length);
                }
                else
                {
                    //
                    // in normal practice we'll never see this...
                    //
                    BigInteger tmp = this.remainder(m);

                    //zAccum = new int[m.magnitude.Length * 2];
                    zVal = new int[m._magnitude.Length];

                    Array.Copy(tmp._magnitude, 0, zVal, zVal.Length - tmp._magnitude.Length,
                            tmp._magnitude.Length);
                }

                yAccum = new int[m._magnitude.Length * 2];
            }

            var yVal = new int[m._magnitude.Length];

            //
            // from LSW to MSW
            //
            for (int i = 0; i < exponent._magnitude.Length; i++)
            {
                int v = exponent._magnitude[i];
                int bits = 0;

                if (i == 0)
                {
                    while (v > 0)
                    {
                        v <<= 1;
                        bits++;
                    }

                    //
                    // first time in initialise y
                    //
                    Array.Copy(zVal, 0, yVal, 0, zVal.Length);

                    v <<= 1;
                    bits++;
                }

                while (v != 0)
                {
                    if (useMonty)
                    {
                        // Montgomery square algo doesn't exist, and a normal
                        // square followed by a Montgomery reduction proved to
                        // be almost as heavy as a Montgomery mulitply.
                        multiplyMonty(yAccum, yVal, yVal, m._magnitude, mQ);
                    }
                    else
                    {
                        Square(yAccum, yVal);
                        remainder(yAccum, m._magnitude);
                        Array.Copy(yAccum, yAccum.Length - yVal.Length, yVal, 0, yVal.Length);
                        zero(yAccum);
                    }
                    bits++;

                    if (v < 0)
                    {
                        if (useMonty)
                        {
                            multiplyMonty(yAccum, yVal, zVal, m._magnitude, mQ);
                        }
                        else
                        {
                            multiply(yAccum, yVal, zVal);
                            remainder(yAccum, m._magnitude);
                            Array.Copy(yAccum, yAccum.Length - yVal.Length, yVal, 0,
                                    yVal.Length);
                            zero(yAccum);
                        }
                    }

                    v <<= 1;
                }

                while (bits < 32)
                {
                    if (useMonty)
                    {
                        multiplyMonty(yAccum, yVal, yVal, m._magnitude, mQ);
                    }
                    else
                    {
                        Square(yAccum, yVal);
                        remainder(yAccum, m._magnitude);
                        Array.Copy(yAccum, yAccum.Length - yVal.Length, yVal, 0, yVal.Length);
                        zero(yAccum);
                    }
                    bits++;
                }
            }

            if (useMonty)
            {
                // Return y * R^(-1) Mod m by doing y * 1 * R^(-1) Mod m
                zero(zVal);
                zVal[zVal.Length - 1] = 1;
                multiplyMonty(yAccum, yVal, zVal, m._magnitude, mQ);
            }

            return new BigInteger(1, yVal);
        }

        /**
         * return w with w = x * x - w is assumed to have enough space.
         */
        private static void Square(IList<int> w, IReadOnlyList<int> x)
        {
            long u1,
            u2;

            if (w.Count != 2 * x.Count)
            {
                throw new ArgumentException("no I don't think so...");
            }

            for (var i = x.Count - 1; i != 0; i--)
            {
                var v = (x[i] & IMask);

                u1 = v * v;
                u2 = (long)((ulong)u1 >> 32);
                u1 = u1 & IMask;

                u1 += (w[2 * i + 1] & IMask);

                w[2 * i + 1] = (int)u1;
                var c = u2 + (u1 >> 32);

                for (int j = i - 1; j >= 0; j--)
                {
                    u1 = (x[j] & IMask) * v;
                    u2 = (long)((ulong)u1 >> 31); // multiply by 2!
                    u1 = (u1 & 0x7fffffff) << 1; // multiply by 2!
                    u1 += (w[i + j + 1] & IMask) + c;

                    w[i + j + 1] = (int)u1;
                    c = u2 + (long)((ulong)u1 >> 32);
                }
                c += w[i] & IMask;
                w[i] = (int)c;
                w[i - 1] = (int)(c >> 32);
            }

            u1 = (x[0] & IMask);
            u1 = u1 * u1;
            u2 = (long)((ulong)u1 >> 32);
            u1 = u1 & IMask;

            u1 += (w[1] & IMask);

            w[1] = (int)u1;
            w[0] = (int)(u2 + (u1 >> 32) + w[0]);
        }

        /**
         * return x with x = y * z - x is assumed to have enough space.
         */
        private int[] multiply(int[] x, int[] y, int[] z)
        {
            for (int i = z.Length - 1; i >= 0; i--)
            {
                long a = z[i] & IMask;
                long value = 0;

                for (int j = y.Length - 1; j >= 0; j--)
                {
                    value += a * (y[j] & IMask) + (x[i + j + 1] & IMask);

                    x[i + j + 1] = (int)value;

                    value = (long)((ulong)value >> 32);
                }

                x[i] = (int)value;
            }

            return x;
        }

        /**
         * Calculate mQuote = -m^(-1) Mod b with b = 2^32 (32 = word size)
         */
        private long getMQuote()
        {
            if (_mQuote != -1L)
            { // allready calculated
                return _mQuote;
            }
            if ((_magnitude[_magnitude.Length - 1] & 1) == 0)
            {
                return -1L; // not for even numbers
            }

            byte[] bytes = { 1, 0, 0, 0, 0 };
            BigInteger b = new BigInteger(1, bytes); // 2^32
            _mQuote = this.negate().Mod(b).ModInverse(b).LongValue();
            return _mQuote;
        }

        /**
         * Montgomery multiplication: a = x * y * R^(-1) Mod m
         * <br>
         * Based algorithm 14.36 of Handbook of Applied Cryptography.
         * <br>
         * <li> m, x, y should have length n </li>
         * <li> a should have length (n + 1) </li>
         * <li> b = 2^32, R = b^n </li>
         * <br>
         * The result is put in x
         * <br>
         * NOTE: the indices of x, y, m, a different in HAC and in Java
         */
        public void multiplyMonty(int[] a, int[] x, int[] y, int[] m, long mQuote)
        // mQuote = -m^(-1) Mod b
        {
            int n = m.Length;
            int nMinus1 = n - 1;
            long y_0 = y[n - 1] & IMask;

            // 1. a = 0 (Notation: a = (a_{n} a_{n-1} ... a_{0})_{b} )
            for (int i = 0; i <= n; i++)
            {
                a[i] = 0;
            }

            // 2. for i from 0 to (n - 1) do the following:
            for (int i = n; i > 0; i--)
            {

                long x_i = x[i - 1] & IMask;

                // 2.1 u = ((a[0] + (x[i] * y[0]) * mQuote) Mod b
                long u = ((((a[n] & IMask) + ((x_i * y_0) & IMask)) & IMask) * mQuote) & IMask;

                // 2.2 a = (a + x_i * y + u * m) / b
                long prod1 = x_i * y_0;
                long prod2 = u * (m[n - 1] & IMask);
                long tmp = (a[n] & IMask) + (prod1 & IMask) + (prod2 & IMask);
                long carry = (long)((ulong)prod1 >> 32) + (long)((ulong)prod2 >> 32) + (long)((ulong)tmp >> 32);
                for (int j = nMinus1; j > 0; j--)
                {
                    prod1 = x_i * (y[j - 1] & IMask);
                    prod2 = u * (m[j - 1] & IMask);
                    tmp = (a[j] & IMask) + (prod1 & IMask) + (prod2 & IMask) + (carry & IMask);
                    carry = (long)((ulong)carry >> 32) + (long)((ulong)prod1 >> 32) +
                        (long)((ulong)prod2 >> 32) + (long)((ulong)tmp >> 32);
                    a[j + 1] = (int)tmp; // division by b
                }
                carry += (a[0] & IMask);
                a[1] = (int)carry;
                a[0] = (int)((ulong)carry >> 32); // OJO!!!!!
            }

            // 3. if x >= m the x = x - m
            if (CompareTo(0, a, 0, m) >= 0)
            {
                Subtract(0, a, 0, m);
            }

            // put the result in x
            for (int i = 0; i < n; i++)
            {
                x[i] = a[i + 1];
            }
        }

        public BigInteger multiply(BigInteger val)
        {
            if (_sign == 0 || val._sign == 0)
                return BigInteger.Zero;

            int[] res = new int[_magnitude.Length + val._magnitude.Length];

            return new BigInteger(_sign * val._sign, multiply(res, _magnitude, val._magnitude));
        }

        public BigInteger negate()
        {
            return new BigInteger(-_sign, _magnitude);
        }

        public BigInteger pow(int exp) //throws ArithmeticException
        {
            if (exp < 0)
                throw new ArithmeticException("Negative exponent");
            if (_sign == 0)
                return (exp == 0 ? BigInteger.One : this);

            BigInteger y,
            z;
            y = BigInteger.One;
            z = this;

            while (exp != 0)
            {
                if ((exp & 0x1) == 1)
                {
                    y = y.multiply(z);
                }
                exp >>= 1;
                if (exp != 0)
                {
                    z = z.multiply(z);
                }
            }

            return y;
        }

        /**
         * return x = x % y - done in place (y value preserved)
         */
        private int[] remainder(int[] x, int[] y)
        {
            int xyCmp = CompareTo(0, x, 0, y);

            if (xyCmp > 0)
            {
                int[] c;
                int shift = BitLength(0, x) - BitLength(0, y);

                if (shift > 1)
                {
                    c = shiftLeft(y, shift - 1);
                }
                else
                {
                    c = new int[x.Length];

                    Array.Copy(y, 0, c, c.Length - y.Length, y.Length);
                }

                Subtract(0, x, 0, c);

                int xStart = 0;
                int cStart = 0;

                for (;;)
                {
                    int cmp = CompareTo(xStart, x, cStart, c);

                    while (cmp >= 0)
                    {
                        Subtract(xStart, x, cStart, c);
                        cmp = CompareTo(xStart, x, cStart, c);
                    }

                    xyCmp = CompareTo(xStart, x, 0, y);

                    if (xyCmp > 0)
                    {
                        if (x[xStart] == 0)
                        {
                            xStart++;
                        }

                        shift = BitLength(cStart, c) - BitLength(xStart, x);

                        c = shift == 0 ? shiftRightOne(cStart, c) : shiftRight(cStart, c, shift);

                        if (c[cStart] == 0)
                        {
                            cStart++;
                        }
                    }
                    else if (xyCmp == 0)
                    {
                        for (var i = xStart; i != x.Length; i++)
                        {
                            x[i] = 0;
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (xyCmp == 0)
            {
                for (int i = 0; i != x.Length; i++)
                {
                    x[i] = 0;
                }
            }

            return x;
        }

        public BigInteger remainder(BigInteger val) //throws ArithmeticException
        {
            if (val._sign == 0)
            {
                throw new ArithmeticException("BigInteger: Divide by zero");
            }

            if (_sign == 0)
            {
                return BigInteger.Zero;
            }

            int[] res = new int[this._magnitude.Length];

            Array.Copy(this._magnitude, 0, res, 0, res.Length);

            return new BigInteger(_sign, remainder(res, val._magnitude));
        }

        /**
         * do a left shift - this returns a new array.
         */
        private int[] shiftLeft(int[] mag, int n)
        {
            int nInts = (int)((uint)n >> 5);
            int nBits = n & 0x1f;
            int magLen = mag.Length;
            int[] newMag = null;

            if (nBits == 0)
            {
                newMag = new int[magLen + nInts];
                for (int i = 0; i < magLen; i++)
                {
                    newMag[i] = mag[i];
                }
            }
            else
            {
                int i = 0;
                int nBits2 = 32 - nBits;
                int highBits = (int)((uint)mag[0] >> nBits2);

                if (highBits != 0)
                {
                    newMag = new int[magLen + nInts + 1];
                    newMag[i++] = highBits;
                }
                else
                {
                    newMag = new int[magLen + nInts];
                }

                int m = mag[0];
                for (int j = 0; j < magLen - 1; j++)
                {
                    int next = mag[j + 1];

                    newMag[i++] = (m << nBits) | (int)((uint)next >> nBits2);
                    m = next;
                }

                newMag[i] = mag[magLen - 1] << nBits;
            }

            return newMag;
        }

        public BigInteger shiftLeft(int n)
        {
            if (_sign == 0 || _magnitude.Length == 0)
            {
                return Zero;
            }
            if (n == 0)
            {
                return this;
            }

            if (n < 0)
            {
                return shiftRight(-n);
            }

            return new BigInteger(_sign, shiftLeft(_magnitude, n));
        }

        /**
         * do a right shift - this does it in place.
         */
        private int[] shiftRight(int start, int[] mag, int n)
        {
            int nInts = (int)((uint)n >> 5) + start;
            int nBits = n & 0x1f;
            int magLen = mag.Length;

            if (nInts != start)
            {
                int delta = (nInts - start);

                for (int i = magLen - 1; i >= nInts; i--)
                {
                    mag[i] = mag[i - delta];
                }
                for (int i = nInts - 1; i >= start; i--)
                {
                    mag[i] = 0;
                }
            }

            if (nBits != 0)
            {
                int nBits2 = 32 - nBits;
                int m = mag[magLen - 1];

                for (int i = magLen - 1; i >= nInts + 1; i--)
                {
                    int next = mag[i - 1];

                    mag[i] = (int)((uint)m >> nBits) | (next << nBits2);
                    m = next;
                }

                mag[nInts] = (int)((uint)mag[nInts] >> nBits);
            }

            return mag;
        }

        /**
         * do a right shift by one - this does it in place.
         */
        private int[] shiftRightOne(int start, int[] mag)
        {
            int magLen = mag.Length;

            int m = mag[magLen - 1];

            for (int i = magLen - 1; i >= start + 1; i--)
            {
                int next = mag[i - 1];

                mag[i] = ((int)((uint)m >> 1)) | (next << 31);
                m = next;
            }

            mag[start] = (int)((uint)mag[start] >> 1);

            return mag;
        }

        public BigInteger shiftRight(int n)
        {
            if (n == 0)
            {
                return this;
            }

            if (n < 0)
            {
                return shiftLeft(-n);
            }

            if (n >= BitLength())
            {
                return (this._sign < 0 ? ValueOf(-1) : BigInteger.Zero);
            }

            int[] res = new int[this._magnitude.Length];

            Array.Copy(this._magnitude, 0, res, 0, res.Length);

            return new BigInteger(this._sign, shiftRight(0, res, n));
        }

        public int signum()
        {
            return _sign;
        }

        /**
         * returns x = x - y - we assume x is >= y
         */
        private int[] Subtract(int xStart, int[] x, int yStart, int[] y)
        {
            int iT = x.Length - 1;
            int iV = y.Length - 1;
            long m;
            int borrow = 0;

            do
            {
                m = (((long)x[iT]) & IMask) - (((long)y[iV--]) & IMask) + borrow;

                x[iT--] = (int)m;

                if (m < 0)
                {
                    borrow = -1;
                }
                else
                {
                    borrow = 0;
                }
            } while (iV >= yStart);

            while (iT >= xStart)
            {
                m = (((long)x[iT]) & IMask) + borrow;
                x[iT--] = (int)m;

                if (m < 0)
                {
                    borrow = -1;
                }
                else
                {
                    break;
                }
            }

            return x;
        }

        public BigInteger Subtract(BigInteger val)
        {
            if (val._sign == 0 || val._magnitude.Length == 0)
            {
                return this;
            }
            if (_sign == 0 || _magnitude.Length == 0)
            {
                return val.negate();
            }
            if (val._sign < 0)
            {
                if (this._sign > 0)
                    return this.add(val.negate());
            }
            else
            {
                if (this._sign < 0)
                    return this.add(val.negate());
            }

            BigInteger bigun,
            littlun;
            int compare = CompareTo(val);
            if (compare == 0)
            {
                return Zero;
            }

            if (compare < 0)
            {
                bigun = val;
                littlun = this;
            }
            else
            {
                bigun = this;
                littlun = val;
            }

            int[] res = new int[bigun._magnitude.Length];

            Array.Copy(bigun._magnitude, 0, res, 0, res.Length);

            return new BigInteger(this._sign * compare, Subtract(0, res, 0, littlun._magnitude));
        }

        public byte[] ToByteArray()
        {
            var bitLength = this.BitLength();
            var bytes = new byte[bitLength / 8 + 1];

            var bytesCopied = 4;
            var mag = 0;
            var ofs = _magnitude.Length - 1;
            var carry = 1;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytesCopied == 4 && ofs >= 0)
                {
                    if (_sign < 0)
                    {
                        // we are dealing with a +ve number and we want a -ve one, so
                        // invert the magnitude ints and add 1 (propagating the carry)
                        // to make a 2's complement -ve number
                        var lMag = ~_magnitude[ofs--] & IMask;
                        lMag += carry;
                        carry = (lMag & ~IMask) != 0 ? 1 : 0;
                        mag = (int)(lMag & IMask);
                    }
                    else
                    {
                        mag = _magnitude[ofs--];
                    }
                    bytesCopied = 1;
                }
                else
                {
                    mag = (int)((uint)mag >> 8);
                    bytesCopied++;
                }

                bytes[i] = (byte)mag;
            }

            return bytes;
        }

        public override string ToString()
        {
            return ToString(10);
        }

        public string ToString(int rdx)
        {
            string format;
            switch (rdx)
            {
                case 10:
                    format = "d";
                    break;
                case 16:
                    format = "x";
                    break;
                default:
                    throw new FormatException("Only base 10 or 16 are allowed");
            }

            if (_magnitude == null)
            {
                return "null";
            }
            else if (_sign == 0)
            {
                return "0";
            }

            var s = "";

            if (rdx == 16)
            {
                s = _magnitude.Select(t => "0000000" + t.ToString("x")).Select(h => h.Substring(h.Length - 8)).Aggregate(s, (current, h) => current + h);
            }
            else
            {
                // This is algorithm 1a from chapter 4.4 in Seminumerical Algorithms, slow but it works
                Stack S = new Stack();
                BigInteger bs = new BigInteger(rdx.ToString());
                // The sign is handled separatly.
                // Notice however that for this to work, radix 16 _MUST_ be a special case,
                // unless we want to enter a recursion well. In their infinite wisdom, why did not 
                // the Sun engineers made a c'tor for BigIntegers taking a BigInteger as parameter?
                // (Answer: Becuase Sun's BigIntger is clonable, something bouncycastle's isn't.)
                BigInteger u = new BigInteger(this.abs().ToString(16), 16);
                BigInteger b;

                // For speed, maye these test should look directly a u.magnitude.Length?
                while (!u.Equals(BigInteger.Zero))
                {
                    b = u.Mod(bs);
                    if (b.Equals(BigInteger.Zero))
                        S.Push("0");
                    else
                    {
                        // see how to interact with different bases
                        S.Push(b._magnitude[0].ToString(format));
                    }
                    u = u.Divide(bs);
                }
                // Then pop the stack
                while (S.Count != 0)
                    s = s + S.Pop();
            }
            // Strip leading zeros.
            while (s.Length > 1 && s[0] == '0')
                s = s.Substring(1);

            if (s.Length == 0)
                s = "0";
            else if (_sign == -1)
                s = "-" + s;

            return s;
        }

        public static readonly BigInteger Zero = new BigInteger(0, new byte[0]);
        public static readonly BigInteger One = ValueOf(1);
        private static readonly BigInteger Two = ValueOf(2);

        public static BigInteger ValueOf(long val)
        {
            if (val == 0)
            {
                return Zero;
            }

            // store val into a byte array
            var b = new byte[8];
            for (var i = 0; i < 8; i++)
            {
                b[7 - i] = (byte)val;
                val >>= 8;
            }

            return new BigInteger(b);
        }

        private int Max(int a, int b)
        {
            return a < b ? b : a;
        }

        public int GetLowestSetBit()
        {
            if (this.Equals(Zero))
            {
                return -1;
            }

            int w = _magnitude.Length - 1;

            while (w >= 0)
            {
                if (_magnitude[w] != 0)
                {
                    break;
                }

                w--;
            }

            var b = 31;

            while (b > 0)
            {
                if ((uint)(_magnitude[w] << b) == 0x80000000)
                {
                    break;
                }

                b--;
            }

            return (((_magnitude.Length - 1) - w) * 32 + (31 - b));
        }

        public bool TestBit(int n) //throws ArithmeticException
        {
            if (n < 0)
            {
                throw new ArithmeticException("Bit position must not be negative");
            }

            if ((n / 32) >= _magnitude.Length)
            {
                return _sign < 0;
            }

            return ((_magnitude[(_magnitude.Length - 1) - n / 32] >> (n % 32)) & 1) > 0;
        }


    }
}
