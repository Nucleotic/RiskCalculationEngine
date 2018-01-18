namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class CalculationResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the base rate.
        /// </summary>
        /// <value>
        /// The base rate.
        /// </value>
        public decimal BaseRate { get; set; }

        /// <summary>
        /// Gets or sets the calculated rate.
        /// </summary>
        /// <value>
        /// The calculated rate.
        /// </value>
        public decimal CalculatedRate { get; set; }

        /// <summary>
        /// Gets or sets the loaded factor.
        /// </summary>
        /// <value>
        /// The loaded factor.
        /// </value>
        public decimal LoadedFactor { get; set; }

        /// <summary>
        /// Gets or sets the calculated annual premium.
        /// </summary>
        /// <value>
        /// The calculated annual premium.
        /// </value>
        public decimal CalculatedAnnualPremium { get; set; }

        /// <summary>
        /// Gets or sets the calculated premium.
        /// </summary>
        /// <value>
        /// The calculated premium.
        /// </value>
        public decimal CalculatedMonthlyPremium { get; set; }

        /// <summary>
        /// Gets or sets the discretionary floor amount.
        /// </summary>
        /// <value>
        /// The discretionary floor amount.
        /// </value>
        public decimal DiscretionaryFloorAmountAnnual { get; set; }

        /// <summary>
        /// Gets or sets the discretionary floor amount monthly.
        /// </summary>
        /// <value>
        /// The discretionary floor amount monthly.
        /// </value>
        public decimal DiscretionaryFloorAmountMonthly { get; set; }


    }
}