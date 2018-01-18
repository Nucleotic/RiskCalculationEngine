using System;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class FlatRateLoadingsCalculationCommand : CalculatorCommandBase<LoadingsContext>
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                GetCalculatorFunctionSet(assembly.Version, "FlatRateLoadings");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var factory = assembly.LoadingsCalculatorFactory;
                    var calculator = factory?.Create("FlatRateLoadings", assembly) as IFlatRateLoadingsCalculator;
                    if (calculator == null) return;
                    dynamic function;
                    switch (assembly.FlatRateCalculationType)
                    {
                        case FlatRateCalculationTypes.None:
                        case FlatRateCalculationTypes.GoodsInTransit:
                        case FlatRateCalculationTypes.CoverAdditions:
                            function = FunctionSet.BaseFlatRateCalculation;
                            assembly.BaseRate = calculator.CalculateFlatRateLoading(function);
                            assembly.Loadings.Add(new AssetLoading { LoadingName = nameof(CalculatorFunctionType.FlatRate), LoadingValue = assembly.BaseRate, DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1 });
                            function = FunctionSet.MonthlyMinimumCalculation;
                            assembly.Loadings.Add(
                                new AssetLoading { LoadingName = nameof(CalculatorFunctionType.MinimumAmount), LoadingValue = calculator.CalculateFlatRateLoading(function), DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1 });
                            function = FunctionSet.MonthlyPremiumCalculation;
                            assembly.CalculatedMonthlyPremium = calculator.CalculateFlatRateMonthlyPremium(function);
                            break;
                        case FlatRateCalculationTypes.Fleet:
                            function = FunctionSet.BandedBaseFlatRateCalculation;
                            assembly.BaseRate = calculator.CalculateFlatRateLoading(function);
                            assembly.Loadings.Add(new AssetLoading { LoadingName = nameof(CalculatorFunctionType.FlatRate), LoadingValue = assembly.BaseRate, DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1 });
                            function = FunctionSet.BaseRateAnnualPremiumCalculation;
                            assembly.CalculatedAnnualPremium = calculator.CalculateBaseRateAnnualPremium(function);
                            assembly.CalculatedMonthlyPremium = assembly.CalculatedAnnualPremium / 12;
                            break;
                        case FlatRateCalculationTypes.FleetGoodsInTransit:
                            function = FunctionSet.BandedBaseFlatRateCalculation;
                            assembly.BaseRate = calculator.CalculateFlatRateLoading(function);
                            assembly.Loadings.Add(new AssetLoading { LoadingName = nameof(CalculatorFunctionType.FlatRate), LoadingValue = assembly.BaseRate, DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1 });
                            function = FunctionSet.BaseRateAnnualPremiumCalculation;
                            assembly.CalculatedAnnualPremium = calculator.CalculateBaseRateAnnualPremium(function);
                            assembly.CalculatedMonthlyPremium = assembly.CalculatedAnnualPremium / 12;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    assembly.DiscretionaryFloorAmountAnnual = assembly.CalculatedAnnualPremium;
                    assembly.DiscretionaryFloorAmountMonthly = assembly.CalculatedMonthlyPremium;
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, ex.Message);
                    assembly.Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(CreditShortfallRateCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
            }
        }
    }
}