using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class AdditionalExcessWaiverCalculationCommand : UnderwritingCalculatorCommandBase
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                GetCalculatorFunctionSet(assembly.Version, "GeneralUnderwritingLoadings");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var calculator = GetCalculatorFromFactory(assembly);
                    if (calculator == null) return;
                    var function = FunctionSet.AdditionalExcessWaiverCalculation;
                    assembly.AdditionalExcess = calculator.CalculateAdditionalExcessWaiverLoading(function);
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.AdditionalExcessWaiver), LoadingValue = assembly.AdditionalExcess, DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1});
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