using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class ExtendedRuleAttribute : RuleAttribute
    {
        /// <summary>
        /// Gets or sets the extensions.
        /// </summary>
        /// <value>
        /// The extensions.
        /// </value>
        public IEnumerable<RuleTypeExtension> Extensions { get; set; }
    }
}
