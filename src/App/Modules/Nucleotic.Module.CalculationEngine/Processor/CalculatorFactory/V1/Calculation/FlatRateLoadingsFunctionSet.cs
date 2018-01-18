using System;
using System.Collections.Generic;
using System.Linq;
using Nucleotic.Common.Extensions;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Calculation
{
    public class FlatRateLoadingsFunctionSet
    {
        /// <summary>
        /// The base flat rate calculation
        /// </summary>
        public Func<IEnumerable<RuleAttribute>, decimal> BaseFlatRateCalculation = (rules) =>
        {
            var rate = rules.Where(r => r.AttributeKey == "FlatRate").Select(r => decimal.Parse(r.AttributeValue)).First();
            return rate / 100;
        };

        /// <summary>
        /// The banded base flat rate calculation
        /// </summary>
        public Func<decimal, string, IEnumerable<BandedRuleAttribute>, decimal> BandedBaseFlatRateCalculation = (cover, product, bandedRules) =>
        {
            var rules = bandedRules as IList<BandedRuleAttribute> ?? bandedRules.ToList();
            var bands = rules.Select(r => r.Bands).First().Where(b => b.BandName.Equals(product, StringComparison.InvariantCulture));
            var band = bands.FirstOrDefault(b => cover >= b.MinValue && (cover <= b.MaxValue || b.MaxValue == null));
            if (band == null) throw new ArgumentOutOfRangeException($"{nameof(cover)}", "BandedBaseFlatRateCalculation out of band or acceptable range");
            var rate = band.Loading;
            return rate;
        };

        /// <summary>
        /// The monthly premium calculation
        /// </summary>
        public Func<decimal, IEnumerable<AssetLoading>, decimal> MonthlyPremiumCalculation = (sumInsured, loadings) =>
        {
            var assetLoadings = loadings as IList<AssetLoading> ?? loadings.ToList();
            var loading = assetLoadings.Where(l => l.LoadingName == nameof(CalculatorFunctionType.FlatRate)).Select(l => l.LoadingValue).FirstOrDefault();
            var minimum = assetLoadings.Where(l => l.LoadingName == nameof(CalculatorFunctionType.MinimumAmount)).Select(l => l.LoadingValue).FirstOrDefault();
            if (loading == default(decimal) || minimum == default(decimal)) throw new ArgumentOutOfRangeException($"{nameof(loadings)}", "FlatRateMonthlyPremiumCalculation does not have a default loading.");
            var amount = sumInsured * loading;
            return Math.Max(amount, minimum);
        };

        /// <summary>
        /// The base rate annual premium calculation
        /// </summary>
        public Func<decimal, decimal, decimal, int, decimal> BaseRateAnnualPremiumCalculation = (sumInsured, baseRate, minimum, count) =>
        {
            var amount = sumInsured * baseRate * count;
            return minimum != default(decimal) ? amount > minimum ? amount : minimum : amount;
        };

        /// <summary>
        /// The monthly minimum calculation
        /// </summary>
        public Func<IEnumerable<RuleAttribute>, decimal> MonthlyMinimumCalculation = (rules) =>
        {
            var amount = rules.Where(r => r.AttributeKey == "MinimumAmount").Select(r => decimal.Parse(r.AttributeValue)).First();
            return amount;
        };

        /// <summary>
        /// The product base premium calculation
        /// </summary>
        public Func<string, IEnumerable<ExtendedRuleAttribute>, decimal> ProductBasePremiumCalculation = (productName, attributes) =>
        {
            var basePremium = attributes.First().Extensions.First(e => e.Criteria.Equals(productName, StringComparison.InvariantCulture)).CriteriaFactors;
            return basePremium;
        };

        /// <summary>
        /// The product-specific flat rate calculation
        /// </summary>
        public Func<decimal, int, decimal> ProductMonthlyPremiumCalculation = (basePremium, factor) =>
        {
            var amount = basePremium * factor;
            return amount;
        };
    }
}
