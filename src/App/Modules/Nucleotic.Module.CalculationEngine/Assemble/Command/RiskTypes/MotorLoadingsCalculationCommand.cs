using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.RiskTypes
{
    public class MotorLoadingsCalculationCommand : CalculatorCommandBase<LoadingsContext>
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                GetCalculatorFunctionSet(assembly.Version, "MotorLoadings");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var factory = assembly.LoadingsCalculatorFactory;
                    var calculator = factory?.Create("MotorLoadings", assembly) as IMotorLoadingsCalculator;
                    if (calculator == null) return;
                    var function = FunctionSet.VehicleUseLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.VehicleUse), LoadingValue = calculator.CalculateVehicleUseLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.VehicleMakeModelLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.VehicleMakeModel), LoadingValue = calculator.CalculateVehicleMakeModelLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.VehicleTypeLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.VehicleType), LoadingValue = calculator.CalculateVehicleTypeLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.VehicleTrackerLoadingCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.VehicleSecurity), LoadingValue = calculator.CalculateVehicleTrackerLoading(function), Order = assembly.Loadings.Count + 1});
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, ex.Message);
                    assembly.Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(MotorLoadingsCalculationCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
            }
        }
    }
}