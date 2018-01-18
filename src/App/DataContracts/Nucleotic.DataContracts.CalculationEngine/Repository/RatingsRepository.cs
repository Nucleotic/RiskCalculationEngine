using Nucleotic.DataContracts.CalculationEngine.Model;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using EFCache;
using Nucleotic.DataContracts.CalculationEngine.Model.ModelExtensions;
using Nucleotic.Framework.Logging;

namespace Nucleotic.DataContracts.CalculationEngine.Repository
{
    public class RatingsRepository : RepositoryBase, IRatingsRepository
    {
        public RatingsRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingsRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RatingsRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets the ratings engine configuration.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="engineType">Type of the engine.</param>
        /// <returns></returns>
        /// <exception cref="EntityException">Active ratings engine configuration not found.</exception>
        public EngineConfiguration GetRatingsEngineConfiguration(int? version, string engineType)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = version.HasValue
                        ? DbEntities.EngineConfigurations.FirstOrDefaultCache(e => e.RulesetVersion == version.Value && e.EngineType == engineType)
                        : DbEntities.EngineConfigurations.Where(e => e.IsActive && e.EngineType == engineType).MaxBy(e => e.RulesetVersion);
                    if (result?.Id == null) throw new EntityException("Active ratings engine configuration not found.");
                    //&& e.EngineType == Enum.GetName(typeof(EngineType), type))
                    return result;
                }
            }
            catch (EntityException ex)
            {
                Logger.LogFatalErrorMessage(ex.Message, ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all rule types.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public IEnumerable<RuleType> GetAllRuleTypes(int version)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result =
                        DbEntities.RuleTypes.Where(rt => rt.Version == version && rt.IsEnabled)
                            .Include(rt => rt.BandValues)
                            .Include(rt => rt.RuleTypeExtensions)
                            .Include(rt => rt.RuleTypeAttributes)
                            .Include(rt => rt.EngineConfiguration)
                            .ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the type of the rule types by criteria.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="criteriaType">Type of the criteria.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IEnumerable<RuleType> GetRuleTypesByCriteriaType(int version, DataRuleTypes criteriaType, EngineType type)
        {
            try
            {
                var criteria = Enum.GetName(typeof(DataRuleTypes), criteriaType);
                var engineType = Enum.GetName(typeof(EngineType), type);
                using (GetEnginesEntitiesConnection())
                {
                    var result =
                        DbEntities.RuleTypes.Where(
                                rt => rt.Version == version && rt.CriteriaSource == criteria && rt.IsEnabled && rt.EngineConfiguration.EngineType == engineType)
                            .Include(rt => rt.BandValues)
                            .Include(rt => rt.RuleTypeExtensions)
                            .Include(rt => rt.RuleTypeAttributes)
                            .ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all broker loadings.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public IEnumerable<BrokerLoading> GetAllBrokerLoadings(bool includeInactive = false)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.BrokerLoadings.Where(bl => bl.IsLatest && bl.DateDeleted == null).ToList();
                    return includeInactive ? result : result.Where(bl => bl.IsActive);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all vehicle loadings.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VehicleLoading> GetAllVehicleLoadings()
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.VehicleLoadings.Where(vl => vl.IsLatest && vl.DateDeleted == null).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all claims history loadings.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IEnumerable<ClaimsHistoryLoading> GetAllClaimsHistoryLoadings(int version, EngineType type)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var engineType = Enum.GetName(typeof(EngineType), type);
                    var result = DbEntities.ClaimsHistoryLoadings
                        .Where(ch => ch.Version == version && ch.RuleType.EngineConfiguration.EngineType == engineType).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all underwriting exceptions.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public IEnumerable<UnderwritingExclusion> GetAllUnderwritingExclusions(int version)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.UnderwritingExclusions.Where(ue => ue.Version == version).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the type of all underwriting exceptions by.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public IEnumerable<UnderwritingExclusion> GetAllUnderwritingExceptionsByRuleName(int version, IList<string> types)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.UnderwritingExclusions.Where(ue => ue.Version == version && types.Contains(ue.RuleType.RuleName)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all underwriting exceptions by rule attribute.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public IEnumerable<UnderwritingExclusion> GetAllUnderwritingExceptionsByRuleAttribute(int version, IList<string> types)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.UnderwritingExclusions.Where(ue => ue.Version == version && types.Contains(ue.RuleTypeAttribute.AttributeKey)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all underwriting exceptions by criteria.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="types">The types.</param>
        /// <param name="engineType">Type of the engine.</param>
        /// <returns></returns>
        public IEnumerable<UnderwritingExclusion> GetAllUnderwritingExceptionsByCriteria(int version, IList<string> types, EngineType engineType)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var et = Enum.GetName(typeof(EngineType), engineType);
                    var result = DbEntities.UnderwritingExclusions
                        .Where(ue => ue.Version == version && types.Contains(ue.RuleType.RuleName))
                        .Where(r => r.RuleType.EngineConfiguration.EngineType == et)
                        .ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the type of all loadings of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllDataOfType<T>()
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var dbset = DbEntities.Set(typeof(T));
                    var result = dbset.OfType<T>().Select(e => e).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

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
        public IEnumerable<PolicyRuleConfiguration> GetPolicyRuleConfigurationsByParameters(int version, string policyNumber, decimal basicSumInsured, string coverType, string postalCode, int brokerId, long? riskItemId, decimal voluntaryExcess)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.PolicyRuleConfigurations.AsNoTracking().NotCached().Where(rc =>
                                rc.RulesVersion == version && rc.PolicyNo == policyNumber && decimal.Round(rc.BasicSumInsured, 2) == (decimal) basicSumInsured &&
                                rc.RiskItemCoverType == coverType && rc.BrokerId == brokerId && rc.PostalCode == postalCode && rc.AdditionalExcess == voluntaryExcess);
                    if (riskItemId.HasValue) result = result.Where(r => r.RiskItemId == riskItemId.Value);
                    return result.Include(rc => rc.PolicyRuleLoadings).Include(rc => rc.PolicyRuleParameters).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the parameters for rule configuration.
        /// </summary>
        /// <param name="policyRuleId">The policy rule identifier.</param>
        /// <returns></returns>
        public IEnumerable<PolicyRuleParameter> GetParametersForRuleConfiguration(int policyRuleId)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    var result = DbEntities.PolicyRuleParameters.Where(vp => vp.PolicyRuleId == policyRuleId);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Saves the policy asset configuration.
        /// </summary>
        /// <param name="assetConfiguration">The asset configuration.</param>
        /// <returns></returns>
        public bool SavePolicyAssetConfiguration(PolicyRuleConfiguration assetConfiguration)
        {
            try
            {
                using (GetEnginesEntitiesConnection())
                {
                    DbEntities.PolicyRuleConfigurations.Add(assetConfiguration);
                    DbEntities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                return false;
            }
        }


    }
}