using Nucleotic.Common.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public partial class RuleType
    {
        /// <summary>
        /// Gets the type of the data rule.
        /// </summary>e
        /// <value>
        /// The type of the data rule.
        /// </value>
        [NotMapped]
        public DataRuleTypes DataRuleType => CriteriaSource.ParseEnum<DataRuleTypes>();

        /// <summary>
        /// Gets the type of the calculate.
        /// </summary>
        /// <value>
        /// The type of the calculate.
        /// </value>
        [NotMapped]
        public CalculationTypes CalcType => CalculationType.ParseEnum<CalculationTypes>();
    }
}