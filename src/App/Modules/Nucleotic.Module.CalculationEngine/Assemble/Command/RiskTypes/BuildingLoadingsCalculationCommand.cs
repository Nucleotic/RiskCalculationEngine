using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Base;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.RiskTypes
{
    public class BuildingLoadingsCalculationCommand : CalculatorCommandBase<LoadingsContext>
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                GetCalculatorFunctionSet(assembly.Version, "BuildingLoadings");
                if (FunctionSet == null) throw new NullReferenceException("Calculator function set is null. Cannot perform calculations.");
                try
                {
                    var factory = assembly.LoadingsCalculatorFactory;
                    var calculator = factory?.Create("BuildingLoadings", assembly) as IBuildingLoadingsCalculator;
                    if (calculator == null) return;
                    var function = FunctionSet.OccupancyTypeCalculation;
                    assembly.Loadings.Add(new AssetLoading { LoadingName = nameof(CalculatorFunctionType.OccupancyType), LoadingValue = calculator.CalculateOccupancyTypeLoading(function), Order = assembly.Loadings.Count + 1 });
                    function = FunctionSet.ConstructionTypeCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.ConstructionType), LoadingValue = calculator.CalculateConstructionTypeLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.BuildingTypeCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.BuildingType), LoadingValue = calculator.CalculateBuildingTypeLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.RoofTypeCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.RoofType), LoadingValue = calculator.CalculateRoofTypeLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.WallTypeCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.WallType), LoadingValue = calculator.CalculateWallTypeLoading(function), Order = assembly.Loadings.Count + 1});
                    function = FunctionSet.BuildingSecurityFeaturesCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.BuildingSecurity), LoadingValue = calculator.CalculateBuildingSecurityLoading(function), Order = assembly.Loadings.Count + 1});
                    if (assembly.PolicyRiskItemRatingType != RiskItemRatingType.Buildings) return;
                    function = FunctionSet.SubsidenceLandslipCoverCalculation;
                    assembly.Loadings.Add(new AssetLoading {LoadingName = nameof(CalculatorFunctionType.SubsistenceLandSlip), LoadingValue = calculator.CalculateSubsidenceLoading(function), Order = assembly.Loadings.Count + 1});
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, ex.Message);
                    assembly.Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(BuildingLoadingsCalculationCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
            }
        }
    }
}