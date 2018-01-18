using Nucleotic.DataContracts.CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nucleotic.DataContracts.CalculationEngine;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Calculation
{
    public class CreditRelatedFunctionSet
    {
        /// <summary>
        /// The cresta zone loading calculation
        /// </summary>
        public Func<int, decimal, IEnumerable<BandRange>, IEnumerable<CrestaZone>, decimal> BasePremiumRateCalculationMotor = (postalCode, cover, bands, crestaAreas) =>
        {
            if (postalCode == -1)
            {
                var extension = bands.First().Extensions.First(e => e.Criteria == "DefaultLoading");
                return extension.CriteriaFactors;
            }
            else
            {
                var zoneNumber = crestaAreas.Where(ca => int.Parse(ca.PostalCode) == postalCode).Select(ca => ca.ZoneNumber).FirstOrDefault();
                var band = bands.FirstOrDefault(b => cover >= b.MinValue && cover <= b.MaxValue);
                if (band == null) throw new ArgumentOutOfRangeException($"{nameof(cover)}", "BasePremiumRateCalculationMotor out of band or acceptable range");
                var extension = band.Extensions.FirstOrDefault(e => e.Criteria.Contains(zoneNumber.ToString()));
                if (extension != null) return band.Loading * extension.CriteriaFactors;
                throw new ArgumentOutOfRangeException($"{nameof(postalCode)}", "BasePremiumRateCalculationMotor out of band or acceptable range");
            }
        };

        /// <summary>
        /// The base premium rate calculation non motor
        /// </summary>
        public Func<int, IEnumerable<ExtendedRuleAttribute>, IEnumerable<CrestaZone>, decimal> BasePremiumRateCalculationNonMotor = (postalCode, attributes, crestaAreas) =>
        {
            RuleTypeExtension extension;
            var ruleAttributes = attributes as IList<ExtendedRuleAttribute> ?? attributes.ToList();
            if (postalCode == -1)
                extension = ruleAttributes.First().Extensions.First(e => e.Criteria == "DefaultLoading");
            else
            {

                var zoneNumber = crestaAreas.Where(ca => int.Parse(ca.PostalCode) == postalCode).Select(ca => ca.ZoneNumber).FirstOrDefault();
                extension = ruleAttributes.First().Extensions.FirstOrDefault(e => e.Criteria.Contains(zoneNumber.ToString()));
            }
            if (extension == null) throw new ArgumentOutOfRangeException($"{nameof(postalCode)}", "BasePremiumRateCalculationNonMotor out of band or acceptable range");
            var baseRate = decimal.Parse(ruleAttributes.First().AttributeValue, CultureInfo.InvariantCulture) * extension.CriteriaFactors;
            return baseRate;
        };

        /// <summary>
        /// The risk item type base premium rate calculation
        /// </summary>
        public Func<AllRiskType, IEnumerable<RuleAttribute>, decimal> BasePremiumRateCalculationRiskItemType = (itemType, rules) =>
        {
            decimal loading;
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            if (!decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == Enum.GetName(typeof(AllRiskType), itemType)).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(itemType)}", "RiskItemTypeBasePremiumRateCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The calculated premium rate calculation
        /// </summary>
        public Func<decimal, decimal, decimal> CalculatedPremiumRateCalculation = (baseCover, annualPremium) =>
        {
            var rate = annualPremium / baseCover * 100;
            return rate;
        };

        /// <summary>
        /// The credit short fall calculation
        /// </summary>
        public Func<bool, decimal, IEnumerable<RuleAttribute>, decimal> CreditShortFallRateCalculation = (hasCover, baseRate, rules) => hasCover
                ? rules.Where(r => r.AttributeKey == "Yes").Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).First() * baseRate
                : rules.Where(r => r.AttributeKey == "No").Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).First();

        /// <summary>
        /// The loaded factor premium calculation
        /// </summary>
        public Func<decimal, decimal, decimal, decimal> LoadedFactorPremiumCalculation = (baseCover, baseRate, factor) =>
        {
            var basePremium = baseCover * (baseRate / 100);
            var premium = basePremium * factor;
            return premium;
        };

        /// <summary>
        /// The monthly premium calculation
        /// </summary>
        public Func<decimal, decimal> MonthlyPremiumCalculation = (annualPremium) => annualPremium / 12;

        /// <summary>
        /// The loaded factor calculation
        /// </summary>
        public Func<IEnumerable<AssetLoading>, decimal> LoadedFactorCalculation = (loadings) =>
        {
            var factor = loadings.Where(l => !l.DoNotAggregateValue).Select(l => l.LoadingValue).Aggregate((initial, next) => initial * next);
            return factor;
        };

        /// <summary>
        /// The discretionary floor amount calculation
        /// </summary>
        public Func<decimal, int, IEnumerable<BrokerLoading>, decimal> DiscretionaryFloorAmountCalculation = (annualPremium, riskItemBroker, brokers) =>
        {
            var brokerLoadings = brokers as IList<BrokerLoading> ?? brokers.ToList();
            var broker = brokerLoadings.FirstOrDefault(b => b.BrokerId == riskItemBroker);
            if (broker == null) return annualPremium;
            {
                var discount = brokerLoadings.Where(b => b.BrokerId == riskItemBroker)
                    .Select(b => b.DiscretionaryDiscount)
                    .First();
                var amount = annualPremium - annualPremium * discount;
                return amount;
            }
        };

        /// <summary>
        /// The monthly floor amount calculation
        /// </summary>
        public Func<decimal, decimal> MonthlyFloorAmountCalculation = (annualPremium) => annualPremium / 12;
    }
}