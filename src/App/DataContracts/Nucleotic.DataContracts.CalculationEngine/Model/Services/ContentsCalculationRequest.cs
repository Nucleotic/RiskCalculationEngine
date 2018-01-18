namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class ContentsCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the building details.
        /// </summary>
        /// <value>
        /// The building details.
        /// </value>
        public BuildingDetailsRequest BuildingDetails { get; set; }
    }
}
