using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration
{
    public class LoadPolicyConfigurationCommand : AssemblorCommand<LoadingsContext>
    {
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                var provider = assembly.RatingsProviderCatalog.RatingsDataProvider;
                var policyConfigurations = provider.GetPolicyRuleConfigurationsByParameters(assembly.Version, assembly.PolicyNumber, assembly.BasicSumInsured,
                    assembly.RiskItemCoverType.ToString(), assembly.PostalCode, assembly.Broker.BrokerId, assembly.RiskItemId, assembly.AdditionalExcess).ToList();
                if (!policyConfigurations.Any()) return;
                switch (assembly.PolicyRiskItemRatingType)
                {
                    case RiskItemRatingType.MotorVehicle:
                        var av = assembly.GetVehicleDetails();
                        var vehicles = new List<PolicyRuleParameter>();
                        foreach (var policyRuleConfiguration in policyConfigurations)
                            vehicles.AddRange(provider.GetParametersForRuleConfiguration(policyRuleConfiguration.Id).ToList());
                        var vehicle = new VehicleDetails
                        {
                            RiskItemCoverType = (RiskCoverType)Enum.Parse(typeof(RiskCoverType), vehicles.Where(v => v.ParameterName.Equals("RiskItemCoverType")).Select(v => v.ParameterValue).First()),
                            IsHighPerformance = bool.Parse(vehicles.Where(v => v.ParameterName.Equals("IsHighPerformance")).Select(v => v.ParameterValue).First()),
                            Make = vehicles.Where(v => v.ParameterName.Equals("Make")).Select(v => v.ParameterValue).First(),
                            Model = vehicles.Where(v => v.ParameterName.Equals("Model")).Select(v => v.ParameterValue).First(),
                            VehicleUse = (ScaleOfUse)Enum.Parse(typeof(ScaleOfUse), vehicles.Where(v => v.ParameterName.Equals("VehicleUse")).Select(v => v.ParameterValue).First()),
                            VehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), vehicles.Where(v => v.ParameterName.Equals("VehicleType")).Select(v => v.ParameterValue).First()),
                            Year = int.Parse(vehicles.Where(v => v.ParameterName.Equals("Year")).Select(v => v.ParameterValue).First()),
                            SecurityFeatures = from feature in vehicles.Where(v => v.ParameterName.Equals("SecurityFeatures")).Select(v => v.ParameterValue).First().Split('|', ';')
                                               select (SecurityFeatures)Enum.Parse(typeof(SecurityFeatures), feature),
                            LoadedFromPreviousCalculation = true
                        };
                        assembly.AssetDetails = vehicle;
                        break;
                    case RiskItemRatingType.NoneSpecified:
                    case RiskItemRatingType.Buildings:
                    case RiskItemRatingType.AllRisk:
                        goto default;
                    default:
                        break;
                }
                assembly.IsLoaded = true;
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                assembly.Errors.Add(ex);
            }
        }

        /// <summary>
        ///     Maps the previous calculations.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="asset">The asset.</param>
        private static void MapPreviousCalculations(LoadingsContext assembly, PolicyRuleConfiguration asset)
        {
            assembly.BaseCalculationsDone = true;
            assembly.BaseRate = asset.BaseRate ?? default(decimal);
            assembly.CalculatedRate = asset.CalculatedRate ?? default(decimal);
            assembly.CalculatedAnnualPremium = asset.CalculatedAnnualPremium ?? default(decimal);
            assembly.CalculatedMonthlyPremium = asset.CalculatedMonthlyPremium ?? default(decimal);
            assembly.CalculationDate = asset.DateCalculated;
            assembly.IsDeclinedReferred = asset.DeclinedReferred;
            assembly.DiscretionaryFloorAmountAnnual = asset.DiscretionaryFloorAmount ?? default(decimal);
            assembly.DiscretionaryFloorAmountMonthly = assembly.DiscretionaryFloorAmountAnnual / 12;
            var loadings = asset.PolicyRuleLoadings.Select(policyRuleLoading => new AssetLoading
            {
                LoadingName = policyRuleLoading.LoadingName,
                LoadingValue = policyRuleLoading.LoadingValue,
                DoNotAggregateValue = policyRuleLoading.IsAggregable
            }).ToList();
            assembly.LoadedFactor = asset.LoadedFactor ?? default(decimal);
            assembly.Loadings = loadings;
        }
    }
}