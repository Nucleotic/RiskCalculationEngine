namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class AllRiskDetails
    {
        /// <summary>
        /// Gets or sets the type of the risk item.
        /// </summary>
        /// <value>
        /// The type of the risk item.
        /// </value>
        public AllRiskType RiskItemType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [loaded from previous calculation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [loaded from previous calculation]; otherwise, <c>false</c>.
        /// </value>
        public bool LoadedFromPreviousCalculation { get; set; }
    }
}
