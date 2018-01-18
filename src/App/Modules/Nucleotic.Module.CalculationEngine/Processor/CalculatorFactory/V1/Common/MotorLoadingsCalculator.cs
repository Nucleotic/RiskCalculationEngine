using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Common
{
    public class MotorLoadingsCalculator : IMotorLoadingsCalculator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public LoadingsContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorLoadingsCalculator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MotorLoadingsCalculator(LoadingsContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Calculates the vehicle use loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateVehicleUseLoading(Func<ScaleOfUse, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var vehicle = Context.GetVehicleDetails();
            var loading = calculator(vehicle.VehicleUse, Context.Attributes.Where(a => a.AttributeName == nameof(CalculatorFunctionType.VehicleUse)));
            return loading;
        }

        /// <summary>
        /// Calculates the vehicle make model loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateVehicleMakeModelLoading(Func<string, string, bool, IEnumerable<VehicleLoading>, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var vehicle = Context.GetVehicleDetails();
            var loading = calculator(vehicle.Make, vehicle.Model, vehicle.IsHighPerformance, Context.VehicleLoadings, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Vehicles the type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateVehicleTypeLoading(Func<VehicleType, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var vehicle = Context.GetVehicleDetails();
            var loading = calculator(vehicle.VehicleType, Context.Attributes.Where(a => a.AttributeName == nameof(CalculatorFunctionType.VehicleType)));
            return loading;
        }

        /// <summary>
        /// Vehicles the tracker loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateVehicleTrackerLoading(Func<bool, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var vehicle = Context.GetVehicleDetails();
            var loading = calculator(vehicle.SecurityFeatures.Contains(SecurityFeatures.Tracker), Context.Attributes.Where(a => a.AttributeName == nameof(CalculatorFunctionType.VehicleSecurity)));
            return loading;
        }


    }
}