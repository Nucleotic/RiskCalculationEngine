using System;
using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public abstract class BaseResponse
    {
        /// <summary>
        /// Gets or sets the basic sum insured.
        /// </summary>
        /// <value>
        /// The basic sum insured.
        /// </value>
        public decimal BasicSumInsured { get; set; }

        /// <summary>
        /// Gets or sets the policy number.
        /// </summary>
        /// <value>
        /// The policy number.
        /// </value>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Gets or sets the asset extension identifier.
        /// </summary>
        /// <value>
        /// The asset extension identifier.
        /// </value>
        public int RiskItemId { get; set; }

        /// <summary>
        /// Gets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        public DateTime CalculationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is declined referred.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is declined referred; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeclinedReferred { get; set; }

        /// <summary>
        /// Gets or sets the declined referred reason.
        /// </summary>
        /// <value>
        /// The declined referred reason.
        /// </value>
        public string DeclinedReferredReason { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IEnumerable<ExceptionMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the loadings.
        /// </summary>
        /// <value>
        /// The loadings.
        /// </value>
        public IEnumerable<AssetLoading> Loadings { get; set; }

    }
}
