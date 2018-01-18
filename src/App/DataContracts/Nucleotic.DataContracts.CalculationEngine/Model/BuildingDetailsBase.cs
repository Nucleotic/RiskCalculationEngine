using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public abstract class BuildingDetailsBase
    {
        /// <summary>
        /// Gets or sets the type of the asset cover.
        /// </summary>
        /// <value>
        /// The type of the asset cover.
        /// </value>
        public RiskCoverType RiskItemCoverType { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the type of the wall.
        /// </summary>
        /// <value>
        /// The type of the wall.
        /// </value>
        public string WallType { get; set; }

        /// <summary>
        /// Gets or sets the type of the roof.
        /// </summary>
        /// <value>
        /// The type of the roof.
        /// </value>
        public string RoofType { get; set; }

        /// <summary>
        /// Gets or sets the type of the building.
        /// </summary>
        /// <value>
        /// The type of the building.
        /// </value>
        public string BuildingType { get; set; }

        /// <summary>
        /// Gets or sets the type of the building construction.
        /// </summary>
        /// <value>
        /// The type of the building construction.
        /// </value>
        public string BuildingConstructionType { get; set; }

        /// <summary>
        /// Gets or sets the security features.
        /// </summary>
        /// <value>
        /// The security features.
        /// </value>
        public IEnumerable<string> SecurityFeatures { get; set; }

        /// <summary>
        /// Gets or sets the type of the occupancy.
        /// </summary>
        /// <value>
        /// The type of the occupancy.
        /// </value>
        public string OccupancyType { get; set; }
    }
}
