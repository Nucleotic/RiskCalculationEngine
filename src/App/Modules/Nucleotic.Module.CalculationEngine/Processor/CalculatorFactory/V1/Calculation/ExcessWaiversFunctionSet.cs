using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Calculation
{
    public class ExcessWaiversFunctionSet
    {
        /// <summary>
        /// The basic excess waiver calculation
        /// </summary>
        public Func<decimal, IEnumerable<BandRange>, decimal> BasicExcessWaiverMonthlyCalculation = (baseCover, bands) =>
        {
            var band = bands.FirstOrDefault(b => baseCover >= b.MinValue && baseCover <= b.MaxValue);
            if (band == null)
                throw new ArgumentOutOfRangeException($"{nameof(baseCover)}", "BasicExcessWaiverMonthlyCalculation out of band or acceptable range");
            return band.Loading;
        };

        /// <summary>
        /// The basic excess waiver annual calculation
        /// </summary>
        public Func<decimal, decimal> BasicExcessWaiverAnnualCalculation = (basicExcessWaiver) =>
        {
            var waiver = basicExcessWaiver * 12;
            return waiver;

        };

        /// <summary>
        /// The excess amount calculation
        /// </summary>
        public Func<decimal, int, IEnumerable<RuleAttribute>, decimal> ExcessAmountCalculation = (baseCover, policyAge, attributes) =>
        {
            var attribute = attributes.FirstOrDefault(a => a.AttributeKey == policyAge.ToString());
            if (attribute == null)
                throw new ArgumentOutOfRangeException($"{nameof(policyAge)}", "ExcessAmountCalculation out of band or acceptable range");
            var amount = decimal.Parse(attribute.AttributeValue, CultureInfo.InvariantCulture) * baseCover;
            return amount / 100;
        };


        /// <summary>
        /// The non motor excess waiver monthly calculation
        /// </summary>
        public Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> NonMotorExcessWaiverMonthlyCalculation;


        /// <summary>
        /// The total loss excess waiver monthly calculation
        /// </summary>
        public Func<decimal, int, Dictionary<int, RuleTypeExtension>, RiskItemRatingType, decimal> TotalLossExcessWaiverMonthlyCalculation = (baseCover, factorCount, extensions, ratingType) =>
        {
            decimal waiver = 0.0m;
            var factor = extensions[0].CriteriaFactors;
            switch (ratingType)
            {
                case RiskItemRatingType.HouseholdContents:
                case RiskItemRatingType.Buildings:
                    waiver = (factor * factorCount) / 12;
                    break;
                case RiskItemRatingType.AllRisk:
                    waiver = factor / 12;
                    break;
                case RiskItemRatingType.MotorVehicle:
                    waiver = extensions[1].CriteriaFactors * (decimal)Math.Pow((double)factor, factorCount > 0 ? factorCount - 1 : factorCount);
                    break;
                case RiskItemRatingType.NoneSpecified:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(ratingType)}", ratingType, null);
            }
            return waiver;
        };

        /// <summary>
        /// The total loss excess waiver annual calculation
        /// </summary>
        public Func<decimal, decimal> ExcessWaiverAnnualCalculation = (totalLossExcessWaiver) =>
        {
            var waiver = totalLossExcessWaiver * 12;
            return waiver;
        };

        public ExcessWaiversFunctionSet()
        {
            NonMotorExcessWaiverMonthlyCalculation = (baseCover, factorCount, extensions, ratingType) => TotalLossExcessWaiverMonthlyCalculation(
                baseCover, factorCount, extensions, ratingType);
        }
    }
}
