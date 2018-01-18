using System.Runtime.Serialization;

namespace Nucleotic.Framework.Logging.LogEntries
{
    [DataContract]
    public sealed class CodeLocation
    {
        #region Class Name

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        [DataMember]
        public string ClassName
        {
            get;
            set;
        }

        #endregion Class Name

        #region Method Name

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        [DataMember]
        public string MethodName
        {
            get;
            set;
        }

        #endregion Method Name

        #region File Name

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [DataMember]
        public string FileName
        {
            get;
            set;
        }

        #endregion File Name

        #region Line Number

        /// <summary>
        /// Gets or sets the line number of the location.
        /// </summary>
        /// <value>The line number.</value>
        [DataMember]
        public int LineNumber
        {
            get;
            set;
        }

        #endregion Line Number

        #region To String

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents
        /// the current <see cref="CodeLocation"/>.
        /// The format is &lt;FileName&gt;(&lt;LineNumber&gt;):&lt;ClassName&gt;.&lt;MethodName&gt;
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="CodeLocation"/>.
        /// </returns>
        public override string ToString()
        {
            return $"{FileName ?? "Nil"}({(LineNumber == -1 ? 0 : LineNumber)}): {ClassName}.{MethodName}";
        }

        #endregion To String
    }
}