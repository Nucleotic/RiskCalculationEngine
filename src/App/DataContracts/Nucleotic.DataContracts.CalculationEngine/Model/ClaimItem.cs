namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class ClaimItem : ClaimItemBase
    {
        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public ClaimType ClaimType { get; set; }
    }
}