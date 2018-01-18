namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class ClaimItemRequest : ClaimItemBase
    {
        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType { get; set; }
    }
}