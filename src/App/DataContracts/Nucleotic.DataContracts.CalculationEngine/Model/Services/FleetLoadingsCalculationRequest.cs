namespace iSixty.DataContracts.RatingEngine.Model.Services
{
    public class FleetLoadingsCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the product count.
        /// </summary>
        /// <value>
        /// The product count.
        /// </value>
        public int ProductCount { get; set; }

        /// <summary>
        /// Gets or sets the type of the risk item product.
        /// </summary>
        /// <value>
        /// The type of the risk item product.
        /// </value>
        public string VehicleType { get; set; }
    }
}
