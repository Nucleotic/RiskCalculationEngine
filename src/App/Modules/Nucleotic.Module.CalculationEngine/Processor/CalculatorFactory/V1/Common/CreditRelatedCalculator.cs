using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Common
{
    public class CreditRelatedCalculator : ICreditRelatedLoadingsCalculator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public LoadingsContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditRelatedCalculator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CreditRelatedCalculator(LoadingsContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Calculates the loaded factor.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateLoadedFactor(Func<IEnumerable<AssetLoading>, decimal> calculator)
        {
            var loadedFactor = calculator(Context.Loadings);
            return loadedFactor;
        }

        /// <summary>
        /// Calculates the base premium rate.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBasePremiumRateMotor(Func<int, decimal, IEnumerable<BandRange>, IEnumerable<CrestaZone>, decimal> calculator)
        {
            var rate = calculator(string.IsNullOrEmpty(Context.PostalCode) ? -1 : int.Parse(Context.PostalCode), (decimal) Context.BasicSumInsured, Context.ExtendedBandValues.Where(bv => bv.BandName == nameof(CalculatorFunctionType.CrestaArea)).ToList(), Context.CrestaZones);
            return rate;
        }

        /// <summary>
        /// Calculates the base premium rate non motor.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBasePremiumRateNonMotor(Func<int, IEnumerable<ExtendedRuleAttribute>, IEnumerable<CrestaZone>, decimal> calculator)
        {
            var rate = calculator(string.IsNullOrEmpty(Context.PostalCode) ? -1 : int.Parse(Context.PostalCode), Context.ExtendedAttributes, Context.CrestaZones);
            return rate;
        }

        /// <summary>
        /// Calculates the type of the base premium rate risk item.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBasePremiumRateRiskItemType(Func<AllRiskType, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(Context.GetAllRiskDetails().RiskItemType, Context.Attributes);
            return loading;
        }

        /// <summary>
        /// Calculates the credit short fall rate.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateCreditShortFallRate(Func<bool, decimal, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var rate = calculator(Context.HasSpecializedShortfallCover, Context.BaseRate, Context.Attributes.Where(a => a.AttributeName == nameof(CalculatorFunctionType.CreditShortfall)));
            return rate;
        }

        /// <summary>
        /// Calculates the loaded factor premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateLoadedFactorAnnualPremium(Func<decimal, decimal, decimal, decimal> calculator)
        {
            var premium = calculator(Context.BasicSumInsured, Context.BaseRate, Context.LoadedFactor);
            return premium;
        }

        /// <summary>
        /// Calculates the monthly premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateMonthlyPremium(Func<decimal, decimal> calculator)
        {
            var premium = calculator(Context.CalculatedAnnualPremium);
            return premium;
        }

        /// <summary>
        /// Calculates the average rate on cover.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateAverageRateOnCover(Func<decimal, decimal, decimal> calculator)
        {
            var rate = calculator(Context.BasicSumInsured, Context.CalculatedAnnualPremium);
            return rate;
        }

        /// <summary>
        /// Calculates the discretionary floor amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public decimal CalculateDiscretionaryFloorAmount(Func<decimal, int, IEnumerable<BrokerLoading>, decimal> calculator)
        {
            var amount = calculator(Context.CalculatedAnnualPremium, Context.Broker.BrokerId, Context.BrokerLoadings);
            return amount;
        }

        /// <summary>
        /// Calculates the monthly floor amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public decimal CalculateMonthlyFloorAmount(Func<decimal, decimal> calculator)
        {
            var amount = calculator(Context.DiscretionaryFloorAmountAnnual);
            return amount;
        }
    }
}