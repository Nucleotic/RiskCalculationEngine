using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class BuildingDetails : BuildingDetailsBase
    {
        /// <summary>
        /// Gets or sets the security features.
        /// </summary>
        /// <value>
        /// The security features.
        /// </value>
        public new IEnumerable<SecurityFeatures> SecurityFeatures { get; set; }

        /// <summary>
        /// Gets or sets the cover additions.
        /// </summary>
        /// <value>
        /// The cover additions.
        /// </value>
        public IEnumerable<string> CoverAdditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [loaded from previous calculation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [loaded from previous calculation]; otherwise, <c>false</c>.
        /// </value>
        public bool LoadedFromPreviousCalculation { get; set; }
    }
}
