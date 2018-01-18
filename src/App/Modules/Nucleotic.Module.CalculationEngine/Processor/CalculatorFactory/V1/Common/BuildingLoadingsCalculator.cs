using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Common
{
    public class BuildingLoadingsCalculator : IBuildingLoadingsCalculator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public LoadingsContext Context { get; }

        /// <summary>
        /// Gets the building details.
        /// </summary>
        /// <value>
        /// The building details.
        /// </value>
        private BuildingDetails BuildingDetails => Context.GetBuildingDetails();

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingLoadingsCalculator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public BuildingLoadingsCalculator(LoadingsContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Calculates the building security loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBuildingSecurityLoading(Func<IEnumerable<SecurityFeatures>, IEnumerable<string>, IEnumerable<RuleTypeExtension>, decimal> calculator)
        {
            var loading = calculator(BuildingDetails.SecurityFeatures, BuildingDetails.CoverAdditions, Context.RuleExtensions);
            return loading;
        }

        /// <summary>
        /// Calculates the building type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBuildingTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(BuildingDetails.BuildingType, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Calculates the construction type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateConstructionTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(BuildingDetails.BuildingConstructionType, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Calculates the occupancy type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateOccupancyTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(BuildingDetails.OccupancyType, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Calculates the roof type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateRoofTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(BuildingDetails.RoofType, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Calculates the subsidence loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateSubsidenceLoading(Func<bool, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(Context.HasSpecializedShortfallCover, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Calculates the wall type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateWallTypeLoading(Func<string, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(BuildingDetails.WallType, Context.Attributes);
            return loading;
        }
    }
}
