using Nucleotic.Common.Extensions;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration
{
    public class SavePolicyConfigurationCommand : AssemblorCommand<LoadingsContext>
    {
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                var provider = assembly.RatingsProviderCatalog.RatingsDataProvider;

                //Setup configuration for save
                var loadings = new List<PolicyRuleLoading>();
                loadings.AddRange(from loading in assembly.Loadings
                    select new PolicyRuleLoading
                    {
                        Id = Guid.NewGuid(),
                        LoadingName = loading.LoadingName,
                        LoadingValue = loading.LoadingValue,
                        IsAggregable = !loading.DoNotAggregateValue
                    });
                PolicyRuleConfiguration asset;
                if (assembly.BaseCalculationsDone)
                    asset = provider.GetPolicyRuleConfigurationsByParameters(assembly.Version, assembly.PolicyNumber, assembly.BasicSumInsured,
                            assembly.RiskItemCoverType.ToString(), assembly.PostalCode, assembly.Broker.BrokerId, assembly.RiskItemId, assembly.AdditionalExcess)
                        .ToList().OrderByDescending(pc => pc.DateCalculated).First();
                else
                    asset = new PolicyRuleConfiguration
                    {
                        RiskItemId = assembly.RiskItemId,
                        BaseRate = assembly.BaseRate,
                        CalculatedRate = assembly.CalculatedRate,
                        BasicSumInsured = assembly.BasicSumInsured,
                        CalculatedMonthlyPremium = assembly.CalculatedMonthlyPremium,
                        CalculatedAnnualPremium = assembly.CalculatedAnnualPremium,
                        CallingContextId = assembly.CallingContextId,
                        DateCalculated = assembly.CalculationDate,
                        DeclinedReferred = assembly.IsDeclinedReferred,
                        PolicyNo = assembly.PolicyNumber,
                        RulesVersion = assembly.Version,
                        PolicyRuleLoadings = loadings,
                        BrokerId = assembly.Broker.BrokerId,
                        PostalCode = !string.IsNullOrEmpty(assembly.PostalCode) ? assembly.PostalCode.Trim() : assembly.PostalCode,
                        RiskItemCoverType = assembly.RiskItemCoverType.ToString(),
                        LoadedFactor = assembly.LoadedFactor,
                        AdditionalExcess = assembly.AdditionalExcess,
                        DiscretionaryFloorAmount = assembly.DiscretionaryFloorAmountAnnual,
                        ProductType = assembly.PolicyProductType.ToString()
                    };
                if (!assembly.IsDeclinedReferred)
                {
                    string[] fieldNames;
                    string features;
                    List<PolicyRuleParameter> parameters;
                    switch (assembly.PolicyRiskItemRatingType)
                    {
                        case RiskItemRatingType.MotorVehicle:
                            fieldNames = new[] { "DriverAge", "RiskItemCoverType", "IsHighPerformance", "Make", "Model", "Year", "VehicleType", "VehicleUse" };
                            features = string.Empty;
                            var vehicle = assembly.GetVehicleDetails();
                            parameters = GetPolicyRuleParameters(fieldNames, vehicle);
                            if (vehicle.SecurityFeatures != null && vehicle.SecurityFeatures.Any())
                            {
                                features = vehicle.SecurityFeatures.Aggregate(features, (current, vehicleSecurityFeature) => current + vehicleSecurityFeature.ToString() + ";");
                                features = features.Remove(features.Length - 1, 1);
                                var paramDetails = new PolicyRuleParameter
                                {
                                    Id = Guid.NewGuid(),
                                    ParameterName = "SecurityFeatures",
                                    ParameterValue = features
                                };
                                parameters.Add(paramDetails);
                            }

                            asset.PolicyRuleParameters = parameters;
                            break;
                        case RiskItemRatingType.Buildings:
                        case RiskItemRatingType.HouseholdContents:
                            fieldNames = new[] {"WallType", "RoofType", "BuildingType", "BuildingConstructionType", "OccupancyType"};
                            features = string.Empty;
                            var building = assembly.GetBuildingDetails();
                            parameters = GetPolicyRuleParameters(fieldNames, building);
                            if (building.SecurityFeatures != null && building.SecurityFeatures.Any())
                            {
                                features = building.SecurityFeatures.Aggregate(features, (current, vehicleSecurityFeature) => current + vehicleSecurityFeature.ToString() + ";");
                                features = features.Remove(features.Length - 1, 1);
                                var paramDetails = new PolicyRuleParameter
                                {
                                    Id = Guid.NewGuid(),
                                    ParameterName = "SecurityFeatures",
                                    ParameterValue = features
                                };
                                asset.PolicyRuleParameters.Add(paramDetails);
                            }
                            asset.PolicyRuleParameters = parameters;
                            break;
                        case RiskItemRatingType.AllRisk:
                            fieldNames = new[] {"RiskItemType"};
                            var riskItem = assembly.GetAllRiskDetails();
                            asset.PolicyRuleParameters = GetPolicyRuleParameters(fieldNames, riskItem);
                            break;
                        case RiskItemRatingType.NoneSpecified:
                        case RiskItemRatingType.FlatRate:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(assembly.PolicyRiskItemRatingType));
                    }
                }

                if (!provider.SavePolicyAssetConfiguration(asset))
                    throw new ApplicationException(
                        $"Failed to save policy asset configuration for Policy Number {assembly.PolicyNumber}; Asset Id {assembly.RiskItemId}; Version {assembly.Version}.");
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                assembly.Errors.Add(ex);
            }
        }

        /// <summary>
        ///     Gets the policy rule parameters.
        /// </summary>
        /// <param name="fieldNames">The field names.</param>
        /// <param name="source">The source.</param>
        private static List<PolicyRuleParameter> GetPolicyRuleParameters(IEnumerable<string> fieldNames, object source)
        {
            var parameters = new List<PolicyRuleParameter>();
            fieldNames.ToList().ForEach(fn =>
            {
                try
                {
                    var paramDetails = new PolicyRuleParameter
                    {
                        Id = Guid.NewGuid(),
                        ParameterName = fn,
                        ParameterValue = source.GetPropValue(fn)?.ToString()
                    };
                    parameters.Add(paramDetails);
                }
                catch (Exception)
                {
                    // ReSharper disable once RedundantJumpStatement
                }
            });

            //Add DateCalculated only if we have parameters
            if (!parameters.Any()) return null;
            parameters.Add(new PolicyRuleParameter
            {
                Id = Guid.NewGuid(),
                ParameterName = "DateCalculated",
                ParameterValue = DateTime.Now.ToLongDateString()
            });
            return parameters;
        }
    }
}