namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class BuildingCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has credit shortfall cover.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has credit shortfall cover; otherwise, <c>false</c>.
        /// </value>
        public bool HasSubsidenceCover { get; set; }

        /// <summary>
        /// Gets or sets the additional excess.
        /// </summary>
        /// <value>
        /// The additional excess.
        /// </value>
        public decimal? AdditionalExcess { get; set; }

        /// <summary>
        /// Gets or sets the building details.
        /// </summary>
        /// <value>
        /// The building details.
        /// </value>
        public BuildingDetailsRequest BuildingDetails { get; set; }
    }
}