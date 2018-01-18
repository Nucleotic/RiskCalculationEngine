using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class VehicleDetailsRequest : VehicleDetailsBase
    {
        /// <summary>
        /// Gets or sets the security features.
        /// </summary>
        /// <value>
        /// The security features.
        /// </value>
        public IEnumerable<string> SecurityFeatures { get; set; }

        /// <summary>
        /// Gets or sets the type of the asset cover.
        /// </summary>
        /// <value>
        /// The type of the asset cover.
        /// </value>
        public string RiskItemCoverType { get; set; }
    }
}