namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class AssetLoading
    {
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the name of the loading.
        /// </summary>
        /// <value>
        /// The name of the loading.
        /// </value>
        public string LoadingName { get; set; }

        /// <summary>
        /// Gets or sets the loading value.
        /// </summary>
        /// <value>
        /// The loading value.
        /// </value>
        public decimal LoadingValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [do not aggregate value].
        /// </summary>
        /// <value>
        /// <c>true</c> if [do not aggregate value]; otherwise, <c>false</c>.
        /// </value>
        public bool DoNotAggregateValue { get; set; }
    }
}