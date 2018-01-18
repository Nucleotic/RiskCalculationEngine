using System;
using System.Collections.Generic;
using System.Linq;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Repository;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Helpers
{
    public static class CommandHelpers
    {
        /// <summary>
        /// Set the base configuration up.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="assembly">The assembly.</param>
        internal static void SetupBaseConfiguration(IRatingsRepository provider, BaseContext assembly)
        {
            var configuration = provider.GetRatingsEngineConfiguration(null, Enum.GetName(typeof(EngineType), assembly.EngineType));
            assembly.Version = configuration.RulesetVersion;
            if (configuration.BaseRate.HasValue) assembly.BaseRate = (decimal)configuration.BaseRate;
        }

        /// <summary>
        /// Setups the band values rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static IEnumerable<BandRange> SetupBandValuesRules(IEnumerable<RuleType> ruleTypes, string criteria = "")
        {
            var result = new List<BandRange>();
            foreach (var rule in ruleTypes)
            {
                var values = string.IsNullOrEmpty(criteria) ? rule.BandValues : rule.BandValues.Where(bv => bv.RuleTypeAttribute.AttributeKey == criteria);
                result.AddRange(values.Select(bv => new BandRange
                {
                    BandName = rule.RuleName,
                    MinValue = bv.BandMinValue,
                    MaxValue = bv.BandMaxValue,
                    Loading = bv.LoadingRate
                }));
            }
            return result;
        }

        /// <summary>
        /// Setups the band values rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <param name="extensions">The extensions.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static IEnumerable<BandRange> SetupBandValuesRules(IEnumerable<RuleType> ruleTypes, IEnumerable<RuleTypeExtension> extensions, string criteria = "")
        {
            var result = new List<BandRange>();
            foreach (var rule in ruleTypes)
            {
                var values = string.IsNullOrEmpty(criteria) ? rule.BandValues : rule.BandValues.Where(bv => bv.RuleTypeAttribute.AttributeKey == criteria);
                result.AddRange(values.Select(bv => new BandRange
                {
                    BandName = rule.RuleName,
                    MinValue = bv.BandMinValue,
                    MaxValue = bv.BandMaxValue,
                    Loading = bv.LoadingRate,
                    Extensions = extensions
                }));
            }
            return result;
        }

        /// <summary>
        /// Setups the extended band values rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <returns></returns>
        internal static IEnumerable<BandRange> SetupExtendedBandValuesRules(IEnumerable<RuleType> ruleTypes)
        {
            var result = from rule in ruleTypes
                         from bandValue in rule.BandValues
                         select new BandRange
                         {
                             BandName = rule.RuleName,
                             MinValue = bandValue.BandMinValue,
                             MaxValue = bandValue.BandMaxValue,
                             Loading = bandValue.LoadingRate,
                             Extensions = rule.RuleTypeExtensions
                         };
            return result;
        }

        /// <summary>
        /// Setups the attributes rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <returns></returns>
        internal static IEnumerable<RuleAttribute> SetupAttributesRules(IEnumerable<RuleType> ruleTypes)
        {
            var result = new List<RuleAttribute>();
            foreach (var rule in ruleTypes)
            {
                result.AddRange(rule.RuleTypeAttributes.Select(ruleTypeAttribute => new RuleAttribute
                {
                    AttributeName = rule.RuleName,
                    AttributeKey = ruleTypeAttribute.AttributeKey,
                    AttributeValue = ruleTypeAttribute.AttributeValue,
                    AttributeType = rule.CalcType
                }));
            }
            return result;
        }

        /// <summary>
        /// Setups the extended attributes rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <returns></returns>
        internal static IEnumerable<ExtendedRuleAttribute> SetupExtendedAttributesRules(IEnumerable<RuleType> ruleTypes)
        {
            var result = new List<ExtendedRuleAttribute>();
            foreach (var rule in ruleTypes)
            {
                result.AddRange(rule.RuleTypeAttributes.Select(ruleTypeAttribute => new ExtendedRuleAttribute
                {
                    AttributeName = rule.RuleName,
                    AttributeKey = ruleTypeAttribute.AttributeKey,
                    AttributeValue = ruleTypeAttribute.AttributeValue,
                    AttributeType = rule.CalcType,
                    Extensions = rule.RuleTypeExtensions
                }));
            }
            return result;
        }

        /// <summary>
        /// Setups the banded attributes rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        internal static IEnumerable<BandedRuleAttribute> SetupBandedAttributesRules(IEnumerable<RuleType> ruleTypes, string criteria = "")
        {
            var result = new List<BandedRuleAttribute>();
            var enumerable = ruleTypes as IList<RuleType> ?? ruleTypes.ToList();
            foreach (var rule in enumerable)
            {
                result.AddRange(rule.RuleTypeAttributes.Select(ruleTypeAttribute => new BandedRuleAttribute()
                {
                    AttributeName = rule.RuleName,
                    AttributeKey = ruleTypeAttribute.AttributeKey,
                    AttributeValue = ruleTypeAttribute.AttributeValue,
                    AttributeType = rule.CalcType,
                    Bands = SetupBandValuesRules(enumerable, criteria)
                }));
            }
            return result;
        }

        /// <summary>
        /// Setups the extended banded attributes rules.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        internal static IEnumerable<BandedRuleAttribute> SetupExtendedBandedAttributesRules(IEnumerable<RuleType> ruleTypes, string criteria)
        {
            var result = new List<BandedRuleAttribute>();
            var enumerable = ruleTypes as IList<RuleType> ?? ruleTypes.ToList();
            foreach (var rule in enumerable)
            {
                foreach (var ruleTypeAttribute in rule.RuleTypeAttributes.Where(a => a.AttributeKey == criteria).ToList())
                {
                    var bandedExtension = new BandedRuleAttribute
                    {
                        AttributeName = rule.RuleName,
                        AttributeKey = ruleTypeAttribute.AttributeKey,
                        AttributeValue = ruleTypeAttribute.AttributeValue,
                        AttributeType = rule.CalcType,
                        Bands = new List<BandRange>()
                    };

                    var extensions = rule.RuleTypeExtensions.Where(e => e.RuleAttributeId == ruleTypeAttribute.Id).ToList();
                    var bandRange = (
                        from ruleTypeExtension in extensions
                        let bands = rule.BandValues.Where(bv => bv.RuleAttributeId == ruleTypeAttribute.Id && bv.RuleExtensionId == ruleTypeExtension.Id)
                        from band in bands
                        select new BandRange
                        {
                            BandName = ruleTypeExtension.Criteria,
                            Loading = band.LoadingRate,
                            MinValue = band.BandMinValue,
                            MaxValue = band.BandMaxValue

                        }).ToList();

                    bandedExtension.Bands = bandRange;
                    result.Add(bandedExtension);
                }
            }
            return result;
        }

        /// <summary>
        /// Setups the rule extensions.
        /// </summary>
        /// <param name="ruleTypes">The rule types.</param>
        /// <returns></returns>
        internal static IEnumerable<RuleTypeExtension> SetupRuleExtensions(IEnumerable<RuleType> ruleTypes)
        {
            var result = new List<RuleTypeExtension>();
            foreach (var rule in ruleTypes)
            {
                result.AddRange(rule.RuleTypeExtensions);
            }
            return result;
        }
    }
}