using Nucleotic.Common.Extensions;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Calculation
{
    public class GeneralUnderwritingLoadingsFunctionSet
    {
        /// <summary>
        /// The age loading calculation
        /// </summary>
        public Func<decimal, IEnumerable<BandRange>, decimal> AgeLoadingCalculation = (age, bands) =>
        {
            var band = bands.FirstOrDefault(b => b.MinValue <= age && (b.MaxValue >= age || b.MaxValue == null));
            if (band != null) return band.Loading;
            throw new ArgumentOutOfRangeException($"{nameof(age)}", "AgeLoadingCalculation out of band or acceptable range");
        };

        /// <summary>
        /// The claims history loading calculation
        /// </summary>
        public Func<IEnumerable<ClaimItem>, IEnumerable<ClaimsHistoryLoading>, decimal> ClaimFrequencyLoadingCalculation = (claims, bands) =>
        {
            var validClaims = claims.Distinct().OrderBy(c => c.DateOfLoss).Where(c => c.DateOfLoss >= DateTime.Now.AddYears(-3)).Select(c => c).ToList();
            if (validClaims.Count == 0) return 1.00m;
            dynamic claimGroup = new ExpandoObject();
            claimGroup.OneYearGroup = validClaims.Where(c => c.DateOfLoss >= DateTime.Now.AddYears(-1) && c.DateOfLoss <= DateTime.Now).Select(c => c).Count();
            claimGroup.TwoYearGroup = validClaims.Where(c => c.DateOfLoss >= DateTime.Now.AddYears(-2) && c.DateOfLoss < DateTime.Now.AddYears(-1)).Select(c => c).Count();
            claimGroup.ThreeYearGroup = validClaims.Where(c => c.DateOfLoss >= DateTime.Now.AddYears(-3) && c.DateOfLoss < DateTime.Now.AddYears(-2)).Select(c => c).Count();

            decimal loading;
            var bandTemp = bands.Where(b => b.NumberOfClaims == validClaims.Count).Select(b => b).ToList();
            if (claimGroup.ThreeYearGroup > 0) loading = bandTemp.First(b => b.ClaimPeriod == 36).LoadingRate;
            else if (claimGroup.TwoYearGroup > 0) loading = bandTemp.First(b => b.ClaimPeriod == 24).LoadingRate;
            else loading = bandTemp.First(b => b.ClaimPeriod == 12).LoadingRate;
            return loading;
        };

        /// <summary>
        /// The claims type loading calculation
        /// </summary>
        public Func<IEnumerable<RuleAttribute>, IEnumerable<ClaimItem>, decimal> ClaimTypeLoadingCalculation = (rules, claims) =>
        {
            try
            {
                var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();

                var loadings = claims.Select(claim =>
                    ruleAttributes.Where(a => a.AttributeKey == claim.ClaimType.ToString())
                        .Select(a => decimal.Parse(a.AttributeValue, CultureInfo.InvariantCulture))
                        .First()).ToList();

                var loading = loadings.Aggregate((initial, next) => initial * next);
                return loading;
            }
            catch
            {
                return 1.0m;
            }

        };

        /// <summary>
        /// The claim total calculation
        /// </summary>
        public Func<decimal, decimal, decimal> ClaimTotalLoadingCalculation = (claimTypeLoading, claimFrequency) => claimTypeLoading * claimFrequency;

        /// <summary>
        /// The cover type loading calculation
        /// </summary>
        public Func<RiskCoverType, IEnumerable<RuleAttribute>, decimal> CoverTypeLoadingCalculation = (coverType, rules) =>
        {
            var loading = rules.Where(r => r.AttributeKey == coverType.ToString()).Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).FirstOrNull();
            if (loading == null) throw new ArgumentOutOfRangeException($"{nameof(coverType)}", "CoverTypeLoadingCalculation arguments violates rule constraints");
            return loading.Value;
        };

        /// <summary>
        /// The broker loading calculation
        /// </summary>
        public Func<int, IEnumerable<BrokerLoading>, IEnumerable<RuleAttribute>, RiskItemRatingType, decimal> BrokerLoadingCalculation = (riskItemBroker, brokers, attributes, ratingType) =>
        {
            try
            {
                var loading = brokers.Where(b => b.BrokerId == riskItemBroker).Select(b => ratingType == RiskItemRatingType.MotorVehicle ? b.LoadingRateMotor : b.LoadingRateNonMotor).FirstOrNull();
                if (loading != null) return loading.Value;
                throw new ArgumentOutOfRangeException($"{nameof(riskItemBroker)}", "BrokerLoadingCalculation arguments violates rule constraints");
            }
            catch (Exception)
            {
                var loading = decimal.Parse(attributes.First(a => a.AttributeName == "BrokerLoadings" && a.AttributeKey == "DefaultLoading").AttributeValue, CultureInfo.InvariantCulture);
                return loading;
            }
        };

        /// <summary>
        /// The additional excess calculation
        /// </summary>
        public Func<decimal, IEnumerable<BandRange>, decimal> AdditionalExcessCalculation = (excessAmount, bands) =>
        {
            var band = bands.FirstOrDefault(b => b.MinValue <= excessAmount && (b.MaxValue >= excessAmount || b.MaxValue == null));
            if (band != null) return band.Loading;
            throw new ArgumentOutOfRangeException($"{nameof(excessAmount)}", "AdditionalExcessCalculation out of band or acceptable range");
        };

        /// <summary>
        /// The additional excess waiver calculation
        /// </summary>
        public Func<decimal, string, IEnumerable<RuleAttribute>, decimal> AdditionalExcessWaiverCalculation = (sumInsured, waiverItem, attributes) =>
        {
            var rules = attributes as IList<RuleAttribute> ?? attributes.ToList();
            var value = rules.FirstOrDefault(a => a.AttributeKey == waiverItem && a.AttributeName == "AdditionalExcessWaiver") != null
                ? rules.FirstOrDefault(a => a.AttributeKey == waiverItem && a.AttributeName == "AdditionalExcessWaiver")?.AttributeValue
                : rules.FirstOrDefault(a => a.AttributeKey.Contains("Other") && a.AttributeName == "AdditionalExcessWaiver")?.AttributeValue;
            if (value == null) throw new ArgumentOutOfRangeException($"{nameof(waiverItem)}", "AdditionalExcessWaiverCalculation arguments violates rule constraints");
            var amount = decimal.Parse(value, CultureInfo.InvariantCulture);
            return amount;
        };
    }
}