using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model.Services
{
    public class FlatRateCalculationRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets the type of the flat rate calculation.
        /// </summary>
        /// <value>
        /// The type of the flat rate calculation.
        /// </value>
        public string FlatRateCalculationType { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product count.
        /// </summary>
        /// <value>
        /// The product count.
        /// </value>
        public int ProductCount { get; set; }
    }
}
