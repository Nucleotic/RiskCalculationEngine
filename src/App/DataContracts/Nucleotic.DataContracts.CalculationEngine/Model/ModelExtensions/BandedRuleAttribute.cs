using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class BandedRuleAttribute : RuleAttribute
    {
        /// <summary>
        /// Gets or sets the bands.
        /// </summary>
        /// <value>
        /// The bands.
        /// </value>
        public IEnumerable<BandRange> Bands { get; set; }
    }
}
