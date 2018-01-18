using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class AllRiskCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the type of the risk item.
        /// </summary>
        /// <value>
        /// The type of the risk item.
        /// </value>
        public string RiskItemType { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public new IEnumerable<ClaimItemRequest> Claims { get; set; }
    }
}
