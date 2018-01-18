using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class BandRange
    {
        /// <summary>
        /// Gets or sets the name of the band.
        /// </summary>
        /// <value>
        /// The name of the band.
        /// </value>
        public string BandName { get; set; }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public decimal MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the loading.
        /// </summary>
        /// <value>
        /// The loading.
        /// </value>
        public decimal Loading { get; set; }

        /// <summary>
        /// Gets or sets the extensions.
        /// </summary>
        /// <value>
        /// The extensions.
        /// </value>
        public IEnumerable<RuleTypeExtension> Extensions { get; set; }
    }
}