using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class ExcessWaiversCalculationCommand : ExcessWaiverCalculatorCommandBase
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(ExcessWaiverContext assembly)
        {
            try
            {
                GetCalculatorFunctionSet(assembly.Version, "ExcessWaivers");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var calculator = GetCalculatorFromFactory(assembly);
                    if (calculator == null) return;
                    var function = FunctionSet.ExcessAmountCalculation;
                    assembly.ExcessAmount = calculator.CalculateExcessAmount(function);
                    if (assembly.PolicyRiskItemRatingType == RiskItemRatingType.MotorVehicle)
                    {
                        function = FunctionSet.BasicExcessWaiverMonthlyCalculation;
                        assembly.BasicMonthlyWaiverPremium = calculator.CalculateBasicExcessWaiver(function);
                    }
                    else
                    {
                        function = FunctionSet.NonMotorExcessWaiverMonthlyCalculation;
                        assembly.BasicMonthlyWaiverPremium = calculator.CalculateNonMotorExcessWaiver(function);
                    }
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.BasicExcessWaiver), LoadingValue = assembly.BasicMonthlyWaiverPremium, Order = assembly.Loadings.Count + 1 });
                    function = FunctionSet.BasicExcessWaiverAnnualCalculation;
                    assembly.BasicAnnualisedWaiverPremium = calculator.CalculateAnnualisedBasicExcessWaiver(function);
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.AnnualisedBasicExcessWaiver), LoadingValue = assembly.BasicAnnualisedWaiverPremium, Order = assembly.Loadings.Count + 1 });
                    if (assembly.PolicyRiskItemRatingType != RiskItemRatingType.MotorVehicle) return;
                    function = FunctionSet.TotalLossExcessWaiverMonthlyCalculation;
                    assembly.TotalLossMonthlyWaiverPremium = calculator.CalculateTotalLossExcessWaiver(function);
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.FullLossExcessWaiver), LoadingValue = assembly.TotalLossMonthlyWaiverPremium, Order = assembly.Loadings.Count + 1 });
                    function = FunctionSet.ExcessWaiverAnnualCalculation;
                    assembly.TotalLossAnnualisedWaiverPremium = calculator.CalculateTotalLossAnnualisedExcessWaiver(function);
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.AnnualisedFullLossExcessWaiver), LoadingValue = assembly.TotalLossAnnualisedWaiverPremium, Order = assembly.Loadings.Count + 1 });
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, ex.Message);
                    assembly.Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(ExcessBandLoadingCalculationCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
                throw;
            }
        }
    }
}