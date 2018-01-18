using System;
using Nucleotic.DataContracts.CalculationEngine.Model;

namespace Nucleotic.Module.CalculationEngine.Assemble.Contexts
{
    public partial class LoadingsContext
    {
        /// <summary>
        /// Gets the vehicle details.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Vehicle details on assembly cannot be null or is of invalid type</exception>
        public VehicleDetails GetVehicleDetails()
        {
            var vehicle = AssetDetails != null ? (VehicleDetails)AssetDetails : null;
            if (vehicle == null)
                throw new ArgumentNullException(AssetDetails, "Vehicle details on assembly cannot be null or is of invalid type");
            return vehicle;
        }

        /// <summary>
        /// Gets the building details.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Building details on assembly cannot be null or is of invalid type</exception>
        public BuildingDetails GetBuildingDetails()
        {
            var building = AssetDetails != null ? (BuildingDetails)AssetDetails : null;
            if (building == null)
                throw new ArgumentNullException(AssetDetails, "Building details on assembly cannot be null or is of invalid type");
            return building;
        }

        /// <summary>
        /// Gets all risk details.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">All Risk details on assembly cannot be null or is of invalid type</exception>
        public AllRiskDetails GetAllRiskDetails()
        {
            var allrisk = AssetDetails != null ? (AllRiskDetails)AssetDetails : null;
            if (allrisk == null)
                throw new ArgumentNullException(AssetDetails, "All Risk details on assembly cannot be null or is of invalid type");
            return allrisk;
        }
    }
}
