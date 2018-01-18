using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces
{
    public interface IFlatRateLoadingsCalculator : ICalculator<LoadingsContext>
    {
        /// <summary>
        /// Calculates the flat rate loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateFlatRateLoading(Func<IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the flat rate loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateFlatRateLoading(Func<decimal, string, IEnumerable<BandedRuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the minimum amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateMinimumAmount(Func<IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the flat rate premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateFlatRateMonthlyPremium(Func<decimal, IEnumerable<AssetLoading>, decimal> calculator);

        /// <summary>
        /// Calculates the product specific flat rate premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateProductMontlyPremium(Func<decimal, int, decimal> calculator);

        /// <summary>
        /// Calculates the base rate annual premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBaseRateAnnualPremium(Func<decimal, decimal, decimal, int, decimal> calculator);

        /// <summary>
        /// Calculates the product base premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateProductBasePremium(Func<string, IEnumerable<ExtendedRuleAttribute>, decimal> calculator);
    }
}
