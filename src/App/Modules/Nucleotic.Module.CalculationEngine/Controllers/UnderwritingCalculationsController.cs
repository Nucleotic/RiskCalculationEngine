using Nucleotic.DataContracts.CalculationEngine.Model.Services;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Assemble.Helpers;
using Nucleotic.Module.CalculationEngine.Assemble.Pipeline;
using System;
using System.Linq;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Controllers
{
    public class UnderwritingCalculationsController : Diagnostics<UnderwritingCalculationsController>, IActivityController
    {
        private readonly int _version;

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid Key { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderwritingCalculationsController"/> class.
        /// </summary>
        public UnderwritingCalculationsController() : this(1) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderwritingCalculationsController"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public UnderwritingCalculationsController(int version)
        {
            Key = Guid.NewGuid();
            _version = version;
        }

        /// <summary>
        /// Calculates the motor loadings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        public CalculationResponse CalculateMotorLoadings(VehicleCalculationRequest request, bool useState = true)
        {
            try
            {
                var context = ModelMappingHelpers.MapMotorLoadingsContext(_version, request, request.ConnectionString);
                return RunMotorLoadingsCalculations(context, request, useState);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the building loadings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        public CalculationResponse CalculateBuildingLoadings(BuildingCalculationRequest request, bool useState = true)
        {
            try
            {
                var context = ModelMappingHelpers.MapBuildingLoadingsContext(_version, request, request.ConnectionString);
                return RunBuildingLoadingsCalculations(context, request, useState);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        public CalculationResponse CalculateContentsLoadings(ContentsCalculationRequest request, bool useState = true)
        {
            try
            {
                var context = ModelMappingHelpers.MapContentsLoadingsContext(_version, request, request.ConnectionString);
                return RunContentsCalculations(context, request, useState);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates all risk loadings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        public CalculationResponse CalculateAllRiskLoadings(AllRiskCalculationRequest request, bool useState = true)
        {
            try
            {
                var context = ModelMappingHelpers.MapAllRiskLoadingsContext(_version, request, request.ConnectionString);
                return RunAllRiskLoadingsCalculations(context, request, useState);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the excess waiver.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        public ExcessWaiverCalculationResponse CalculateExcessWaiver(ExcessWaiverCalculationRequest request, bool useState = false)
        {
            try
            {
                var context = ModelMappingHelpers.MapExcessWaiverContext(_version, request.PolicyNumber, request.BasicSumInsured, request.RiskItemId, request.RiskItemFactor,
                    request.RatingType, request.AllRiskItemType, request.ConnectionString);
                return RunExcessWaiverCalculations(context, useState);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates the excess waiver.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        public CalculationResponse CalculateFlatRateLoadings(FlatRateCalculationRequest request, bool useState = false)
        {
            try
            {
                var context = ModelMappingHelpers.MapFlatRateLoadingsContext(_version, request, request.ConnectionString);
                return RunFlatRateCalculations(context, request, useState);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Runs the motor loadings calculations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> uses policy configuration state.</param>
        /// <returns></returns>
        private static CalculationResponse RunMotorLoadingsCalculations(LoadingsContext context, VehicleCalculationRequest request, bool useState)
        {
            var result = new CalculationResponse
            {
                PolicyNumber = context.PolicyNumber,
                RiskItemId = context.RiskItemId,
                CalculationDate = DateTime.Now,
                BasicSumInsured = context.BasicSumInsured,
                IsDeclinedReferred = false
            };

            //Check if Policy configuration exists and if calculations have already run; if so return the configuration
            if (useState)
            {
                var loadPipeline = new PolicyConfigurationLoadPipeline();
                var loadContext = loadPipeline.Run(context);
                if (context.IsLoaded && CheckLoadingsPolicyHasCalculated(loadContext, request))
                {
                    context = loadContext;
                    if ((!context.IsDeclinedReferred && context.GetVehicleDetails().LoadedFromPreviousCalculation) || context.IsDeclinedReferred)
                    {
                        ModelMappingHelpers.MapResultFromContext(context, result);
                        return result;
                    }
                }
            }

            //Validate Policy Extension does not raise exceptions and has valid underwriting ranges
            var checkPipeline = new LoadingsValidationPipeline();
            context = checkPipeline.Run(context);

            if (!context.IsDeclinedReferred)
            {
                //Policy Extension is valid and loadings can be calculated
                var calcPipeline = new MotorLoadingsCalculationPipeline();
                context = calcPipeline.Run(context);
                ModelMappingHelpers.MapResultFromContext(context, result);
            }
            else
            {
                //Policy Extension is invalid and does not have valid ranges
                result.IsDeclinedReferred = true;
                result.DeclinedReferredReason = context.DeclinedReferredReason;
            }

            //Finalize
            if (!useState) return result;
            var savePipeline = new PolicyConfigurationSavePipeline();
            savePipeline.Run(context);
            return result;
        }

        /// <summary>
        /// Runs the building loadings calculations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        private static CalculationResponse RunBuildingLoadingsCalculations(LoadingsContext context, BuildingCalculationRequest request, bool useState)
        {
            var result = new CalculationResponse
            {
                PolicyNumber = context.PolicyNumber,
                RiskItemId = context.RiskItemId,
                CalculationDate = DateTime.Now,
                BasicSumInsured = context.BasicSumInsured,
                IsDeclinedReferred = false
            };

            //Check if Policy configuration exists and if calculations have already run; if so return the configuration
            if (useState)
            {
                var loadPipeline = new PolicyConfigurationLoadPipeline();
                var loadContext = loadPipeline.Run(context);
                if (context.IsLoaded && CheckLoadingsPolicyHasCalculated(loadContext, request))
                {
                    context = loadContext;
                    if ((!context.IsDeclinedReferred && context.GetBuildingDetails().LoadedFromPreviousCalculation) || context.IsDeclinedReferred)
                    {
                        ModelMappingHelpers.MapResultFromContext(context, result);
                        return result;
                    }
                }
            }

            //Validate Policy Extension does not raise exceptions and has valid underwriting ranges
            var checkPipeline = new LoadingsValidationPipeline();
            context = checkPipeline.Run(context);

            if (!context.IsDeclinedReferred)
            {
                //Policy Extension is valid and loadings can be calculated
                var calcPipeline = new BuildingLoadingsCalculationPipeline();
                context = calcPipeline.Run(context);
                ModelMappingHelpers.MapResultFromContext(context, result);
            }
            else
            {
                //Policy Extension is invalid and does not have valid ranges
                result.IsDeclinedReferred = true;
                result.DeclinedReferredReason = context.DeclinedReferredReason;
            }

            //Finalize
            if (!useState) return result;
            var savePipeline = new PolicyConfigurationSavePipeline();
            savePipeline.Run(context);
            return result;
        }

        /// <summary>
        /// Runs the contents calculations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        private static CalculationResponse RunContentsCalculations(LoadingsContext context, ContentsCalculationRequest request, bool useState)
        {
            var result = new CalculationResponse
            {
                PolicyNumber = context.PolicyNumber,
                RiskItemId = context.RiskItemId,
                CalculationDate = DateTime.Now,
                BasicSumInsured = context.BasicSumInsured,
                IsDeclinedReferred = false
            };

            //Check if Policy configuration exists and if calculations have already run; if so return the configuration
            if (useState)
            {
                var loadPipeline = new PolicyConfigurationLoadPipeline();
                var loadContext = loadPipeline.Run(context);
                if (context.IsLoaded && CheckLoadingsPolicyHasCalculated(loadContext, request))
                {
                    context = loadContext;
                    if ((!context.IsDeclinedReferred && context.GetBuildingDetails().LoadedFromPreviousCalculation) || context.IsDeclinedReferred)
                    {
                        ModelMappingHelpers.MapResultFromContext(context, result);
                        return result;
                    }
                }
            }

            //Validate Policy Extension does not raise exceptions and has valid underwriting ranges
            var checkPipeline = new LoadingsValidationPipeline();
            context = checkPipeline.Run(context);

            if (!context.IsDeclinedReferred)
            {
                //Policy Extension is valid and loadings can be calculated
                var calcPipeline = new ContentsLoadingsCalculationPipeline();
                context = calcPipeline.Run(context);
                ModelMappingHelpers.MapResultFromContext(context, result);
            }
            else
            {
                //Policy Extension is invalid and does not have valid ranges
                result.IsDeclinedReferred = true;
                result.DeclinedReferredReason = context.DeclinedReferredReason;
            }

            //Finalize
            if (!useState) return result;
            var savePipeline = new PolicyConfigurationSavePipeline();
            savePipeline.Run(context);
            return result;
        }

        /// <summary>
        /// Runs all risk loadings calculations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        private static CalculationResponse RunAllRiskLoadingsCalculations(LoadingsContext context, AllRiskCalculationRequest request, bool useState)
        {
            var result = new CalculationResponse
            {
                PolicyNumber = context.PolicyNumber,
                RiskItemId = context.RiskItemId,
                CalculationDate = DateTime.Now,
                BasicSumInsured = context.BasicSumInsured,
                IsDeclinedReferred = false
            };

            //Check if Policy configuration exists and if calculations have already run; if so return the configuration
            if (useState)
            {
                var loadPipeline = new PolicyConfigurationLoadPipeline();
                var loadContext = loadPipeline.Run(context);
                if (context.IsLoaded && CheckLoadingsPolicyHasCalculated(loadContext, request))
                {
                    context = loadContext;
                    if ((!context.IsDeclinedReferred && context.GetAllRiskDetails().LoadedFromPreviousCalculation) || context.IsDeclinedReferred)
                    {
                        ModelMappingHelpers.MapResultFromContext(context, result);
                        return result;
                    }
                }
            }

            //Validate Policy Extension does not raise exceptions and has valid underwriting ranges
            var checkPipeline = new LoadingsValidationPipeline();
            context = checkPipeline.Run(context);

            if (!context.IsDeclinedReferred)
            {
                //Policy Extension is valid and loadings can be calculated
                var calcPipeline = new AllRiskLoadingsCalculationPipeline();
                context = calcPipeline.Run(context);
                ModelMappingHelpers.MapResultFromContext(context, result);
            }
            else
            {
                //Policy Extension is invalid and does not have valid ranges
                result.IsDeclinedReferred = true;
                result.DeclinedReferredReason = context.DeclinedReferredReason;
            }

            //Finalize
            if (!useState) return result;
            var savePipeline = new PolicyConfigurationSavePipeline();
            savePipeline.Run(context);
            return result;
        }

        /// <summary>
        /// Runs the excess waiver calculations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        private static ExcessWaiverCalculationResponse RunExcessWaiverCalculations(ExcessWaiverContext context, bool useState)
        {
            var result = new ExcessWaiverCalculationResponse
            {
                PolicyNumber = context.PolicyNumber,
                RiskItemId = context.RiskItemId,
                CalculationDate = DateTime.Now,
                BasicSumInsured = context.BasicSumInsured,
                IsDeclinedReferred = false
            };

            //Validate Policy Extension does not raise exceptions and has valid underwriting ranges
            var checkPipeline = new WaiversValidationPipeline();
            context = checkPipeline.Run(context);

            if (!context.IsDeclinedReferred)
            {
                //Policy Extension is valid and loadings can be calculated
                var calcPipeline = new ExcessWaiverCalculationPipeline();
                context = calcPipeline.Run(context);
                ModelMappingHelpers.MapResultFromContext(context, result);
            }
            else
            {
                //Policy Extension is invalid and does not have valid ranges
                result.IsDeclinedReferred = true;
            }

            //Finalize
            if (!useState) return result;
            return result;
        }

        /// <summary>
        /// Runs the flat rate calculations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="useState">if set to <c>true</c> [use state].</param>
        /// <returns></returns>
        private static CalculationResponse RunFlatRateCalculations(LoadingsContext context, FlatRateCalculationRequest request, bool useState)
        {
            var result = new CalculationResponse
            {
                PolicyNumber = context.PolicyNumber,
                RiskItemId = context.RiskItemId,
                CalculationDate = DateTime.Now,
                BasicSumInsured = context.BasicSumInsured,
                IsDeclinedReferred = false
            };

            //Check if Policy configuration exists and if calculations have already run; if so return the configuration
            if (useState)
            {
                var loadPipeline = new PolicyConfigurationLoadPipeline();
                var loadContext = loadPipeline.Run(context);
                if (context.IsLoaded && CheckLoadingsPolicyHasCalculated(loadContext, request))
                {
                    context = loadContext;
                    if ((!context.IsDeclinedReferred && context.GetVehicleDetails().LoadedFromPreviousCalculation) || context.IsDeclinedReferred)
                    {
                        ModelMappingHelpers.MapResultFromContext(context, result);
                        return result;
                    }
                }
            }

            //Validate Policy Extension does not raise exceptions and has valid underwriting ranges
            //var checkPipeline = new FlatRateLoadingsValidationPipeline();
            //context = checkPipeline.Run(context);

            if (!context.IsDeclinedReferred)
            {
                //Policy Extension is valid and loadings can be calculated
                var calcPipeline = new FlatRateLoadingsCalculationPipeline();
                context = calcPipeline.Run(context);
                ModelMappingHelpers.MapResultFromContext(context, result);
            }
            else
            {
                //Policy Extension is invalid and does not have valid ranges
                result.IsDeclinedReferred = true;
                result.DeclinedReferredReason = context.DeclinedReferredReason;
            }

            //Finalize
            if (!useState) return result;
            var savePipeline = new PolicyConfigurationSavePipeline();
            savePipeline.Run(context);
            return result;
        }

        /// <summary>
        /// Checks the policy has calculated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private static bool CheckLoadingsPolicyHasCalculated(LoadingsContext context, VehicleCalculationRequest request)
        {
            try
            {
                if (context.IsDeclinedReferred) return true;
                var requestContext = ModelMappingHelpers.MapMotorLoadingsContext(context.Version, request);
                //TODO: Use RulesEngine for comparison checks
                var compare = requestContext.RiskItemId == context.RiskItemId &&
                              requestContext.PolicyNumber.Equals(context.PolicyNumber) &&
                              requestContext.HasSpecializedShortfallCover == context.HasSpecializedShortfallCover &&
                              requestContext.AdditionalExcess == context.AdditionalExcess &&
                              requestContext.Broker.BrokerId == context.Broker.BrokerId &&
                              request.PostalCode == context.PostalCode &&
                              requestContext.Claims.ToList().Count == context.Claims.ToList().Count &&
                              requestContext.BasicSumInsured == context.BasicSumInsured &&
                              requestContext.Age == context.Age;
                var check = (context.BaseRate > 0) && (context.CalculatedRate > 0) &&
                            (context.CalculatedAnnualPremium > 0) && (context.CalculatedMonthlyPremium > 0) &&
                            context.BaseCalculationsDone;
                return compare && check;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the loadings policy has calculated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private static bool CheckLoadingsPolicyHasCalculated(LoadingsContext context, BuildingCalculationRequest request)
        {
            try
            {
                if (context.IsDeclinedReferred) return true;
                var requestContext = ModelMappingHelpers.MapBuildingLoadingsContext(context.Version, request);
                //TODO: Use RulesEngine for comparison checks
                var compare = requestContext.RiskItemId == context.RiskItemId &&
                              requestContext.PolicyNumber.Equals(context.PolicyNumber) &&
                              requestContext.HasSpecializedShortfallCover == context.HasSpecializedShortfallCover &&
                              requestContext.AdditionalExcess == context.AdditionalExcess &&
                              requestContext.Broker.BrokerId == context.Broker.BrokerId &&
                              request.PostalCode == context.PostalCode &&
                              requestContext.Claims.ToList().Count == context.Claims.ToList().Count &&
                              requestContext.BasicSumInsured == context.BasicSumInsured &&
                              requestContext.Age == context.Age;
                var check = (context.BaseRate > 0) && (context.CalculatedRate > 0) &&
                            (context.CalculatedAnnualPremium > 0) && (context.CalculatedMonthlyPremium > 0) &&
                            context.BaseCalculationsDone;
                return compare && check;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the loadings policy has calculated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private static bool CheckLoadingsPolicyHasCalculated(LoadingsContext context, ContentsCalculationRequest request)
        {
            try
            {
                if (context.IsDeclinedReferred) return true;
                var requestContext = ModelMappingHelpers.MapContentsLoadingsContext(context.Version, request);
                //TODO: Use RulesEngine for comparison checks
                var compare = requestContext.RiskItemId == context.RiskItemId &&
                              requestContext.PolicyNumber.Equals(context.PolicyNumber) &&
                              requestContext.HasSpecializedShortfallCover == context.HasSpecializedShortfallCover &&
                              requestContext.AdditionalExcess == context.AdditionalExcess &&
                              requestContext.Broker.BrokerId == context.Broker.BrokerId &&
                              request.PostalCode == context.PostalCode &&
                              requestContext.Claims.ToList().Count == context.Claims.ToList().Count &&
                              requestContext.BasicSumInsured == context.BasicSumInsured &&
                              requestContext.Age == context.Age;
                var check = (context.BaseRate > 0) && (context.CalculatedRate > 0) &&
                            (context.CalculatedAnnualPremium > 0) && (context.CalculatedMonthlyPremium > 0) &&
                            context.BaseCalculationsDone;
                return compare && check;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the loadings policy has calculated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private static bool CheckLoadingsPolicyHasCalculated(LoadingsContext context, AllRiskCalculationRequest request)
        {
            try
            {
                if (context.IsDeclinedReferred) return true;
                var requestContext = ModelMappingHelpers.MapAllRiskLoadingsContext(context.Version, request);
                //TODO: Use RulesEngine for comparison checks
                var compare = requestContext.RiskItemId == context.RiskItemId &&
                              requestContext.PolicyNumber.Equals(context.PolicyNumber) &&
                              requestContext.HasSpecializedShortfallCover == context.HasSpecializedShortfallCover &&
                              requestContext.AdditionalExcess == context.AdditionalExcess &&
                              requestContext.Broker.BrokerId == context.Broker.BrokerId &&
                              requestContext.Claims.ToList().Count == context.Claims.ToList().Count &&
                              requestContext.BasicSumInsured == context.BasicSumInsured &&
                              requestContext.Age == context.Age;
                var check = (context.BaseRate > 0) && (context.CalculatedRate > 0) &&
                            (context.CalculatedAnnualPremium > 0) && (context.CalculatedMonthlyPremium > 0) &&
                            context.BaseCalculationsDone;
                return compare && check;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the loadings policy has calculated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private static bool CheckLoadingsPolicyHasCalculated(LoadingsContext context, FlatRateCalculationRequest request)
        {
            try
            {
                if (context.IsDeclinedReferred) return true;
                var requestContext = ModelMappingHelpers.MapFlatRateLoadingsContext(context.Version, request);
                //TODO: Use RulesEngine for comparison checks
                var compare = requestContext.RiskItemId == context.RiskItemId
                              && requestContext.PolicyNumber.Equals(context.PolicyNumber)
                              && requestContext.BasicSumInsured == context.BasicSumInsured
                              && requestContext.Age == context.Age;
                var check = (context.BaseRate > 0) && (context.CalculatedRate > 0) &&
                            (context.CalculatedAnnualPremium > 0) && (context.CalculatedMonthlyPremium > 0) &&
                            context.BaseCalculationsDone;
                return compare && check;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the waivers policy has calculated.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static bool CheckWaiversPolicyHasCalculated(ExcessWaiverContext context)
        {
            if (context.IsDeclinedReferred) return true;
            var check = context.BasicMonthlyWaiverPremium > 0 && context.BasicAnnualisedWaiverPremium > 0 &&
                        context.TotalLossMonthlyWaiverPremium > 0 && context.TotalLossAnnualisedWaiverPremium > 0;
            return check;
        }
    }
}