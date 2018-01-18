using Nucleotic.DataContracts.CalculationEngine.Model;
using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Repository
{
    public interface IRatingsRepository
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the ratings engine configuration.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="">The .</param>
        /// <param name="engineType">Type of the engine.</param>
        /// <returns></returns>
        EngineConfiguration GetRatingsEngineConfiguration(int? version, string engineType);

        /// <summary>
        /// Gets all rule types.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        IEnumerable<RuleType> GetAllRuleTypes(int version);

        /// <summary>
        /// Gets the type of the rule types by criteria.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="criteriaType">Type of the criteria.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IEnumerable<RuleType> GetRuleTypesByCriteriaType(int version, DataRuleTypes criteriaType, EngineType type);

        /// <summary>
        /// Gets all broker loadings.
        /// </summary>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive].</param>
        /// <returns></returns>
        IEnumerable<BrokerLoading> GetAllBrokerLoadings(bool includeInactive = false);

        /// <summary>
        /// Gets all vehicle loadings.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VehicleLoading> GetAllVehicleLoadings();

        /// <summary>
        /// Gets all claims history loadings.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IEnumerable<ClaimsHistoryLoading> GetAllClaimsHistoryLoadings(int version, EngineType type);

        /// <summary>
        /// Gets all underwriting exceptions.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        IEnumerable<UnderwritingExclusion> GetAllUnderwritingExclusions(int version);

        /// <summary>
        /// Gets the type of all underwriting exceptions by.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        IEnumerable<UnderwritingExclusion> GetAllUnderwritingExceptionsByRuleName(int version, IList<string> types);

        /// <summary>
        /// Gets all underwriting exceptions by rule attribute.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        IEnumerable<UnderwritingExclusion> GetAllUnderwritingExceptionsByRuleAttribute(int version, IList<string> types);

        /// <summary>
        /// Gets all underwriting exceptions by criteria.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="types">The types.</param>
        /// <param name="engineType">Type of the engine.</param>
        /// <returns></returns>
        IEnumerable<UnderwritingExclusion> GetAllUnderwritingExceptionsByCriteria(int version, IList<string> types, EngineType engineType);

        /// <summary>
        /// Gets the type of all data of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllDataOfType<T>();

        /// <summary>
        /// Gets the policy rule configurations by policy number.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="policyNumber">The policy number.</param>
        /// <param name="basicSumInsured">The base cover.</param>
        /// <param name="coverType">Type of the cover.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="brokerId">The broker identifier.</param>
        /// <param name="riskItemId">The asset extension identifier.</param>
        /// <param name="voluntaryExcess">The voluntary excess.</param>
        /// <returns></returns>
        IEnumerable<PolicyRuleConfiguration> GetPolicyRuleConfigurationsByParameters(int version, string policyNumber, decimal basicSumInsured, string coverType, string postalCode, int brokerId, long? riskItemId, decimal voluntaryExcess);

        /// <summary>
        /// Gets the parameters for rule configuration.
        /// </summary>
        /// <param name="policyRuleId">The policy rule identifier.</param>
        /// <returns></returns>
        IEnumerable<PolicyRuleParameter> GetParametersForRuleConfiguration(int policyRuleId);

        /// <summary>
        /// Saves the policy asset configuration.
        /// </summary>
        /// <param name="assetConfiguration">The asset configuration.</param>
        /// <returns></returns>
        bool SavePolicyAssetConfiguration(PolicyRuleConfiguration assetConfiguration);
    }
}