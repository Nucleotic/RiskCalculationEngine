using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces
{
    public interface IBuildingLoadingsCalculator : ICalculator<LoadingsContext>
    {
        /// <summary>
        /// Calculates the occupancy type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateOccupancyTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the construction type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateConstructionTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the building type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBuildingTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the roof type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateRoofTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the wall type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateWallTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the building security loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBuildingSecurityLoading(Func<IEnumerable<SecurityFeatures>, IEnumerable<string>, IEnumerable<RuleTypeExtension>, decimal> calculator);

        /// <summary>
        /// Calculates the subsidence loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateSubsidenceLoading(Func<bool, IEnumerable<RuleAttribute>, decimal> calculator);
    }
}
