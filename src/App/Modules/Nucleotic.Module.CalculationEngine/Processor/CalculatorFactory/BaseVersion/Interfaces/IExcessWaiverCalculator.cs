using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces
{
    public interface IExcessWaiverCalculator : ICalculator<ExcessWaiverContext>
    {
        /// <summary>
        /// Calculates the basic excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBasicExcessWaiver(Func<decimal, IEnumerable<BandRange>, decimal> calculator);

        /// <summary>
        /// Calculates the annualized basic excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateAnnualisedBasicExcessWaiver(Func<decimal, decimal> calculator);

        /// <summary>
        /// Calculates the total loss excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateTotalLossExcessWaiver(Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> calculator);

        /// <summary>
        /// Calculates the non motor excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateNonMotorExcessWaiver(Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> calculator);

        /// <summary>
        /// Calculates the non motor annualised excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateNonMotorAnnualisedExcessWaiver(Func<decimal, decimal> calculator);

        /// <summary>
        /// Calculates the total loss annualized excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateTotalLossAnnualisedExcessWaiver(Func<decimal, decimal> calculator);

        /// <summary>
        /// Calculates the excess amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateExcessAmount(Func<decimal, int, IEnumerable<RuleAttribute>, decimal> calculator);
    }
}
