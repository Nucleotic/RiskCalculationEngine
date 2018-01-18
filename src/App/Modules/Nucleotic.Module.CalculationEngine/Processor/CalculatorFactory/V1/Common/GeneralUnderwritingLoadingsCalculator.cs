using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Common
{
    public class GeneralUnderwritingLoadingsCalculator : IGeneralUnderwritingLoadingsCalculator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public LoadingsContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralUnderwritingLoadingsCalculator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GeneralUnderwritingLoadingsCalculator(LoadingsContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Calculates the age loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateAgeLoading(Func<decimal, IEnumerable<BandRange>, decimal> calculator)
        {
            var loading = calculator(Context.GetVehicleDetails().DriverAge, Context.BandValues.Where(bv => bv.BandName == nameof(CalculatorFunctionType.Age)));
            return loading;
        }

        /// <summary>
        /// Calculates the claim frequency loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateClaimFrequencyLoading(Func<IEnumerable<ClaimItem>, IEnumerable<ClaimsHistoryLoading>, decimal> calculator)
        {
            var loading = calculator(Context.Claims, Context.ClaimsHistoryLoadings);
            return loading;
        }

        /// <summary>
        /// Calculates the claim type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateClaimTypeLoading(Func<IEnumerable<RuleAttribute>, IEnumerable<ClaimItem>, decimal> calculator)
        {
            var loading = calculator(Context.Attributes.Where(a => a.AttributeName == nameof(CalculatorFunctionType.ClaimType)), Context.Claims);
            return loading;
        }

        /// <summary>
        /// Calculates the claim total loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateClaimTotalLoading(Func<decimal, decimal, decimal> calculator)
        {
            var loading = calculator(
                    Context.Loadings.First(l => l.LoadingName == nameof(CalculatorFunctionType.ClaimType)).LoadingValue,
                    Context.Loadings.First(l => l.LoadingName == nameof(CalculatorFunctionType.ClaimFrequency)).LoadingValue);
            return loading;
        }

        /// <summary>
        /// Calculates the cover type loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateCoverTypeLoading(Func<RiskCoverType, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(Context.RiskItemCoverType, Context.Attributes.Where(a => a.AttributeName == nameof(CalculatorFunctionType.CoverType)));
            return loading;
        }

        /// <summary>
        /// Calculates the broker loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBrokerLoading(Func<int, IEnumerable<BrokerLoading>, IEnumerable<RuleAttribute>, RiskItemRatingType, decimal> calculator)
        {
            var loading = calculator(Context.Broker.BrokerId, Context.BrokerLoadings, Context.Attributes, Context.PolicyRiskItemRatingType);
            return loading;
        }

        /// <summary>
        /// Calculates the Additional excess loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateAdditionalExcessLoading(Func<decimal, IEnumerable<BandRange>, decimal> calculator)
        {
            var loading = calculator(Context.AdditionalExcess, Context.BandValues.Where(bv => bv.BandName == nameof(CalculatorFunctionType.AdditionalExcess)));
            return loading;
        }

        /// <summary>
        /// Calculates the additional excess waiver loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateAdditionalExcessWaiverLoading(Func<decimal, string, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var itemType = Context.GetAllRiskDetails().RiskItemType == AllRiskType.CellPhone ? "Cellphones" : Context.GetAllRiskDetails().RiskItemType == AllRiskType.Unspecified ? "Unspecified" : "Other";
            var amount = calculator(Context.BasicSumInsured, itemType, Context.Attributes);
            return amount;
        }
    }
}