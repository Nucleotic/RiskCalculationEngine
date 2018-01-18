using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;
using System;
using System.Collections.Generic;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces
{
    public interface IGeneralUnderwritingLoadingsCalculator : ICalculator<LoadingsContext>
    {
        /// <summary>
        /// Calculates the age loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateAgeLoading(Func<decimal, IEnumerable<BandRange>, decimal> calculator);

        /// <summary>
        /// Calculates the claim frequency loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>

        decimal CalculateClaimFrequencyLoading(Func<IEnumerable<ClaimItem>, IEnumerable<ClaimsHistoryLoading>, decimal> calculator);

        /// <summary>
        /// Calculates the claim type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateClaimTypeLoading(Func<IEnumerable<RuleAttribute>, IEnumerable<ClaimItem>, decimal> calculator);

        /// <summary>
        /// Calculates the claim total loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateClaimTotalLoading(Func<decimal, decimal, decimal> calculator);

        /// <summary>
        /// Calculates the cover type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateCoverTypeLoading(Func<RiskCoverType, IEnumerable<RuleAttribute>, decimal> calculator);

        /// <summary>
        /// Calculates the broker loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateBrokerLoading(Func<int, IEnumerable<BrokerLoading>, IEnumerable<RuleAttribute>, RiskItemRatingType, decimal> calculator);

        /// <summary>
        /// Calculates the Additional excess loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateAdditionalExcessLoading(Func<decimal, IEnumerable<BandRange>, decimal> calculator);

        /// <summary>
        /// Calculates the additional excess waiver loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        decimal CalculateAdditionalExcessWaiverLoading(Func<decimal, string, IEnumerable<RuleAttribute>, decimal> calculator);
    }
}