using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;
using System;
using System.Collections.Generic;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces
{
    public interface IMotorLoadingsCalculator : ICalculator<LoadingsContext>
    {
        /// <summary>
        /// Calculates the vehicle use loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateVehicleUseLoading(Func<ScaleOfUse, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the vehicle make model loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateVehicleMakeModelLoading(Func<string, string, bool, IEnumerable<VehicleLoading>, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Vehicles the type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateVehicleTypeLoading(Func<VehicleType, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Vehicles the tracker loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateVehicleTrackerLoading(Func<bool, IEnumerable<RuleAttribute>, decimal> calculator);
    }
}