using System;

namespace Nucleotic.Common.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class CompareExcludeAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether [exclude in comparison].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exclude in comparison]; otherwise, <c>false</c>.
        /// </value>
        public bool ExcludeInComparison { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareExcludeAttribute"/> class.
        /// </summary>
        /// <param name="exclude">if set to <c>true</c> [exclude].</param>
        public CompareExcludeAttribute(bool exclude = true)
        {
            ExcludeInComparison = exclude;
        }
    }
}
