using System;
using System.Linq;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class ClaimsLoadingCalculationCommand : UnderwritingCalculatorCommandBase
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="System.NullReferenceException">Calculator function set is null. Cannot perform calculations.</exception>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                if (assembly.ClaimsCount <= 0 && !assembly.Claims.Any()) return;
                GetCalculatorFunctionSet(assembly.Version, "GeneralUnderwritingLoadings");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var calculator = GetCalculatorFromFactory(assembly);
                    if (calculator == null) return;
                    var function = FunctionSet.ClaimTypeLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.ClaimType), LoadingValue = calculator.CalculateClaimTypeLoading(function), DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.ClaimFrequencyLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.ClaimFrequency), LoadingValue = calculator.CalculateClaimFrequencyLoading(function), DoNotAggregateValue = true, Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.ClaimTotalLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.ClaimTotal), LoadingValue = calculator.CalculateClaimTotalLoading(function), Order = assembly.Loadings.Count + 1});
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