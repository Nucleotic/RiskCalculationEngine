namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class ExcessWaiverCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the type of the rating.
        /// </summary>
        /// <value>
        /// The type of the rating.
        /// </value>
        public string RatingType { get; set; }

        /// <summary>
        /// Gets or sets the type of all risk.
        /// </summary>
        /// <value>
        /// The type of all risk.
        /// </value>
        public string AllRiskItemType { get; set; }

        /// <summary>
        /// Gets or sets the risk item factor.
        /// </summary>
        /// <value>
        /// The risk item factor.
        /// </value>
        public int? RiskItemFactor { get; set; }
    }
}
