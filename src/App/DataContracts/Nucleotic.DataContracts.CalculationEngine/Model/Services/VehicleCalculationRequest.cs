namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class VehicleCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int DriverAge { get; set; }

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
        public bool HasCreditShortfallCover { get; set; }

        /// <summary>
        /// Gets or sets the additional excess.
        /// </summary>
        /// <value>
        /// The additional excess.
        /// </value>
        public decimal AdditionalExcess { get; set; }

        /// <summary>
        /// Gets or sets the vehicle details.
        /// </summary>
        /// <value>
        /// The vehicle details.
        /// </value>
        public VehicleDetailsRequest VehicleDetails { get; set; }
    }
}