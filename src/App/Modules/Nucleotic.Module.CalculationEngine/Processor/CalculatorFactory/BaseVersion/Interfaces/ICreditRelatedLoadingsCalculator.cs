using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;
using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces
{
    public interface ICreditRelatedLoadingsCalculator : ICalculator<LoadingsContext>
    {
        /// <summary>
        /// Calculates the loaded factor.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateLoadedFactor(Func<IEnumerable<AssetLoading>, decimal> calculator);

        /// <summary>
        /// Calculates the base premium rate.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBasePremiumRateMotor(Func<int, decimal, IEnumerable<BandRange>, IEnumerable<CrestaZone>, decimal> calculator);

        /// <summary>
        /// Calculates the base premium rate non motor.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBasePremiumRateNonMotor(Func<int, IEnumerable<ExtendedRuleAttribute>, IEnumerable<CrestaZone>, decimal> calculator);

        /// <summary>
        /// Calculates the type of the base premium rate risk item.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBasePremiumRateRiskItemType(Func<AllRiskType, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the credit short fall rate.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateCreditShortFallRate(Func<bool, decimal, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the loaded factor premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateLoadedFactorAnnualPremium(Func<decimal, decimal, decimal, decimal> calculator);

        /// <summary>
        /// Calculates the monthly premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateMonthlyPremium(Func<decimal, decimal> calculator);

        /// <summary>
        /// Calculates the average rate on cover.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateAverageRateOnCover(Func<decimal, decimal, decimal> calculator);

        /// <summary>
        /// Calculates the discretionary floor amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateDiscretionaryFloorAmount(Func<decimal, int, IEnumerable<BrokerLoading>, decimal> calculator);

        /// <summary>
        /// Calculates the monthly floor amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateMonthlyFloorAmount(Func<decimal, decimal> calculator);
    }
}