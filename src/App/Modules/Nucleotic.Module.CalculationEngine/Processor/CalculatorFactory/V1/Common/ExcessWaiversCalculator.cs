using System;
using System.Collections.Generic;
using System.Linq;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Common
{
    public class ExcessWaiversCalculator : IExcessWaiverCalculator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ExcessWaiverContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcessWaiversCalculator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ExcessWaiversCalculator(ExcessWaiverContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Calculates the basic excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBasicExcessWaiver(Func<decimal, IEnumerable<BandRange>, decimal> calculator)
        {
            var waiver = calculator(Context.BasicSumInsured, Context.BandValues.Where(bv => bv.BandName == nameof(CalculatorFunctionType.BasicExcessWaiver)));
            return waiver;
        }

        /// <summary>
        /// Calculates the annualized basic excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateAnnualisedBasicExcessWaiver(Func<decimal, decimal> calculator)
        {
            var waiver = calculator(Context.BasicMonthlyWaiverPremium);
            return waiver;
        }

        /// <summary>
        /// Calculates the non motor excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateNonMotorExcessWaiver(Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> calculator)
        {
            return CalculateWaiver(calculator);
        }

        /// <summary>
        /// Calculates the non motor annualised excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateNonMotorAnnualisedExcessWaiver(Func<decimal, decimal> calculator)
        {
            var waiver = calculator(Context.BasicMonthlyWaiverPremium);
            return waiver;
        }

        /// <summary>
        /// Calculates the total loss excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateTotalLossExcessWaiver(Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> calculator)
        {
            return CalculateWaiver(calculator);
        }

        private decimal CalculateWaiver(Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> calculator)
        {
            var waiver = 0.0m;
            RuleTypeExtension extension;
            var extensions = Context.ExtendedAttributes.First().Extensions.ToList();
            var input = new Dictionary<int, RuleTypeExtension>();
            switch (Context.PolicyRiskItemRatingType)
            {
                case RiskItemRatingType.MotorVehicle:
                    extension = extensions.First(e => e.Criteria ==
                                                      $"{Enum.GetName(typeof(RiskItemRatingType), Context.PolicyRiskItemRatingType)}Factor");
                    input.Add(input.Count, extension);
                    extension = extensions.First(e => e.Criteria ==
                                                      $"{Enum.GetName(typeof(RiskItemRatingType), Context.PolicyRiskItemRatingType)}FlatRate");
                    input.Add(input.Count, extension);
                    waiver = calculator(Context.BasicSumInsured, Context.PolicyAge, input, Context.PolicyRiskItemRatingType);
                    break;
                case RiskItemRatingType.Buildings:
                case RiskItemRatingType.HouseholdContents:
                    extension = extensions.First(e => e.Criteria ==
                                                      $"{Enum.GetName(typeof(RiskItemRatingType), Context.PolicyRiskItemRatingType)}Factor");
                    input.Add(input.Count, extension);
                    waiver = calculator(Context.BasicSumInsured, Context.RiskAddressCount, input,
                        Context.PolicyRiskItemRatingType);
                    break;
                case RiskItemRatingType.AllRisk:
                    extension = extensions.First(e => e.Criteria ==
                                                      $"{Enum.GetName(typeof(RiskItemRatingType), Context.PolicyRiskItemRatingType)}Factor{Enum.GetName(typeof(AllRiskType), Context.AllRiskType)}");
                    input.Add(input.Count, extension);
                    waiver = calculator(Context.BasicSumInsured, Context.RiskAddressCount, input,
                        Context.PolicyRiskItemRatingType);
                    break;
            }
            return waiver;
        }

        /// <summary>
        /// Calculates the total loss annualized excess waiver.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateTotalLossAnnualisedExcessWaiver(Func<decimal, decimal> calculator)
        {
            var waiver = calculator(Context.TotalLossMonthlyWaiverPremium);
            return waiver;
        }

        /// <summary>
        /// Calculates the excess amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateExcessAmount(Func<decimal, int, IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var amount = calculator(Context.BasicSumInsured, Context.PolicyAge, Context.ExtendedAttributes);
            return amount;
        }
    }
}
