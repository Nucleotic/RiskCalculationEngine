using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class BasePremiumRateCalculationCommand : CreditCalculatorCommandBase
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
                    dynamic function;
                    switch (assembly.EngineType)
                    {
                        case EngineType.MotorLoadings:
                            function = FunctionSet.BasePremiumRateCalculationMotor;
                            assembly.BaseRate = calculator.CalculateBasePremiumRateMotor(function);
                            break;

                        case EngineType.BuildingLoadings:
                        case EngineType.ContentLoadings:
                            function = FunctionSet.BasePremiumRateCalculationNonMotor;
                            assembly.BaseRate = calculator.CalculateBasePremiumRateNonMotor(function);
                            break;

                        case EngineType.AllRiskLoadings:
                            function = FunctionSet.BasePremiumRateCalculationRiskItemType;
                            assembly.BaseRate = calculator.CalculateBasePremiumRateRiskItemType(function) * 10;
                            break;
                        case EngineType.ExcessWaivers:
                        case EngineType.FlatRateLoadings:
                            assembly.BaseRate = 1.00m;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, ex.Message);
                    assembly.Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(BasePremiumRateCalculationCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
                throw;
            }
        }
    }
}