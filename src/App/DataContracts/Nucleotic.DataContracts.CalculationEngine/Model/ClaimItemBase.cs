using System;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public abstract class ClaimItemBase
    {
        /// <summary>
        /// Gets or sets the claim number.
        /// </summary>
        /// <value>
        /// The claim number.
        /// </value>
        public virtual string ClaimNumber { get; set; }

        /// <summary>
        /// Gets or sets the claim date.
        /// </summary>
        /// <value>
        /// The claim date.
        /// </value>
        public virtual DateTime DateOfLoss { get; set; }

        /// <summary>
        /// Gets or sets the claim amount.
        /// </summary>
        /// <value>
        /// The claim amount.
        /// </value>
        public virtual decimal? ClaimAmount { get; set; }

        /// <summary>
        /// Gets or sets the asset description.
        /// </summary>
        /// <value>
        /// The asset description.
        /// </value>
        public virtual string RiskItemDescription { get; set; }
    }
}