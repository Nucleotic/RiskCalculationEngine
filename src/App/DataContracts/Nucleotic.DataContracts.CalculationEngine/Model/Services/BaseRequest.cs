using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public abstract class BaseRequest
    {
        /// <summary>
        /// Gets or sets the base cover.
        /// </summary>
        /// <value>
        /// The base cover.
        /// </value>
        public decimal BasicSumInsured { get; set; }

        /// <summary>
        /// Gets or sets the broker identifier.
        /// </summary>
        /// <value>
        /// The broker identifier.
        /// </value>
        public int BrokerId { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public IEnumerable<ClaimItemRequest> Claims { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the policy number.
        /// </summary>
        /// <value>
        /// The policy number.
        /// </value>
        public string PolicyNumber { get; set; }
        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the type of the asset insurance cover.
        /// </summary>
        /// <value>
        /// The type of the asset insurance cover.
        /// </value>
        public string RiskItemCoverType { get; set; }

        /// <summary>
        /// Gets or sets the asset extension identifier.
        /// </summary>
        /// <value>
        /// The asset extension identifier.
        /// </value>
        public int RiskItemId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use policy configuration state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if using policy configuration state (the default); otherwise, <c>false</c>.
        /// </value>
        public virtual bool UseState { get; set; } = true;
    }
}
