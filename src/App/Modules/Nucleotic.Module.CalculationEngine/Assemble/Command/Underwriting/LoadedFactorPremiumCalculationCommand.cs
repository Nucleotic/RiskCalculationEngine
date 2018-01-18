using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class LoadedFactorPremiumCalculationCommand : CreditCalculatorCommandBase
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                GetCalculatorFunctionSet(assembly.Version, "CreditRelated");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var calculator = GetCalculatorFromFactory(assembly);
                    if (calculator == null) return;
                    var function = FunctionSet.LoadedFactorCalculation;
                    assembly.LoadedFactor = calculator.CalculateLoadedFactor(function);
                    function = FunctionSet.LoadedFactorPremiumCalculation;
                    assembly.CalculatedAnnualPremium = calculator.CalculateLoadedFactorAnnualPremium(function);
                    function = FunctionSet.MonthlyPremiumCalculation;
                    assembly.CalculatedMonthlyPremium = calculator.CalculateMonthlyPremium(function);
                    function = FunctionSet.CalculatedPremiumRateCalculation;
                    assembly.CalculatedRate = calculator.CalculateAverageRateOnCover(function);
                    function = FunctionSet.DiscretionaryFloorAmountCalculation;
                    assembly.DiscretionaryFloorAmountAnnual = calculator.CalculateDiscretionaryFloorAmount(function);
                    function = FunctionSet.MonthlyFloorAmountCalculation;
                    assembly.DiscretionaryFloorAmountMonthly = calculator.CalculateMonthlyFloorAmount(function);
                    var broker = assembly.BrokerLoadings.FirstOrDefault(b => b.BrokerId == assembly.Broker.BrokerId);
                    var discountRate = broker?.DiscretionaryDiscount ?? 1.00m;
                    assembly.Loadings.Add(new AssetLoading
                    {
                        DoNotAggregateValue = true,
                        LoadingName = nameof(CalculatorFunctionType.DiscretionaryFloorDiscount),
                        LoadingValue = discountRate
                    });
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, ex.Message);
                    assembly.Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(AgeBandLoadingCalculationCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
                throw;
            }
        }
    }
}