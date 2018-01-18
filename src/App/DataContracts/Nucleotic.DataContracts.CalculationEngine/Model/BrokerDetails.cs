namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class BrokerDetails
    {
        /// <summary>
        /// Gets or sets the broker identifier.
        /// </summary>
        /// <value>
        /// The broker identifier.
        /// </value>
        public int BrokerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the broker.
        /// </summary>
        /// <value>
        /// The name of the broker.
        /// </value>
        public string BrokerName { get; set; }

        /// <summary>
        /// Gets or sets the loading rate.
        /// </summary>
        /// <value>
        /// The loading rate.
        /// </value>
        public decimal LoadingRate { get; set; }
    }
}