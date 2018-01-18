using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class VehicleDetails : VehicleDetailsBase
    {
        /// <summary>
        /// Gets or sets the type of the vehicle.
        /// </summary>
        /// <value>
        /// The type of the vehicle.
        /// </value>
        public new VehicleType VehicleType { get; set; }

        /// <summary>
        /// Gets or sets the security features.
        /// </summary>
        /// <value>
        /// The security features.
        /// </value>
        public IEnumerable<SecurityFeatures> SecurityFeatures { get; set; }

        /// <summary>
        /// Gets or sets the type of the asset cover.
        /// </summary>
        /// <value>
        /// The type of the asset cover.
        /// </value>
        public RiskCoverType RiskItemCoverType { get; set; }

        /// <summary>
        /// Gets or sets the vehicle use.
        /// </summary>
        /// <value>
        /// The vehicle use.
        /// </value>
        public new ScaleOfUse VehicleUse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [loaded from previous calculation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [loaded from previous calculation]; otherwise, <c>false</c>.
        /// </value>
        public bool LoadedFromPreviousCalculation { get; set; }
    }
}