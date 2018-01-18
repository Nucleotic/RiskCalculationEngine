namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class ExcessWaiverCalculationResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the monthly waiver premium.
        /// </summary>
        /// <value>
        /// The monthly waiver premium.
        /// </value>
        public decimal MonthlyBasicWaiverPremium { get; set; }

        /// <summary>
        /// Gets or sets the annualized waiver premium.
        /// </summary>
        /// <value>
        /// The annualized waiver premium.
        /// </value>
        public decimal AnnualisedBasicWaiverPremium { get; set; }

        /// <summary>
        /// Gets or sets the monthly total loss waiver premium.
        /// </summary>
        /// <value>
        /// The monthly total loss waiver premium.
        /// </value>
        public decimal MonthlyTotalLossWaiverPremium { get; set; }

        /// <summary>
        /// Gets or sets the annualized total loss waiver premium.
        /// </summary>
        /// <value>
        /// The annualized total loss waiver premium.
        /// </value>
        public decimal AnnualisedTotalLossWaiverPremium { get; set; }

        /// <summary>
        /// Gets or sets the excess amount.
        /// </summary>
        /// <value>
        /// The excess amount.
        /// </value>
        public decimal ExcessAmount { get; set; }
    }
}
