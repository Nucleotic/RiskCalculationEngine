using Nucleotic.DataContracts.CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using Nucleotic.Common.Extensions;
using Nucleotic.DataContracts.CalculationEngine;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Calculation
{
    public class BuildingLoadingsFunctionSet
    {
        /// <summary>
        /// The occupancy type calculation
        /// </summary>
        public Func<string, IEnumerable<RuleAttribute>, decimal> OccupancyTypeCalculation = (occupancyType, rules) =>
        {
            decimal loading;
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            if (string.IsNullOrEmpty(occupancyType) || occupancyType.Equals("None"))
                return !decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == "DefaultLoading").AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading) ? 1.00m : loading;
            if (!decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == occupancyType).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(occupancyType)}", "OccupancyTypeCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The construction type calculation
        /// </summary>
        public Func<string, IEnumerable<RuleAttribute>, decimal> ConstructionTypeCalculation = (constructionType, rules) =>
        {
            decimal loading;
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            if (string.IsNullOrEmpty(constructionType) || constructionType.Equals("None"))
                return !decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == "DefaultLoading").AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading) ? 1.00m : loading;
            if (!decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == constructionType).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(constructionType)}", "ConstructionTypeCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The building type calculation
        /// </summary>
        public Func<string, IEnumerable<RuleAttribute>, decimal> BuildingTypeCalculation = (buildingType, rules) =>
        {
            decimal loading;
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            if (string.IsNullOrEmpty(buildingType) || buildingType.Equals("None"))
                return !decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == "DefaultLoading").AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading) ? 1.00m : loading;
            if (!decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == buildingType).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(buildingType)}", "BuildingTypeCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The roof type calculation
        /// </summary>
        public Func<string, IEnumerable<RuleAttribute>, decimal> RoofTypeCalculation = (roofType, rules) =>
        {
            decimal loading;
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            if (string.IsNullOrEmpty(roofType) || roofType.Equals("None"))
                return !decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == "DefaultLoading").AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading) ? 1.00m : loading;
            if (!decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == roofType).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(roofType)}", "RoofTypeCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The wall type calculation
        /// </summary>
        public Func<string, IEnumerable<RuleAttribute>, decimal> WallTypeCalculation = (wallType, rules) =>
        {
            decimal loading;
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            if (string.IsNullOrEmpty(wallType) || wallType.Equals("None"))
                return !decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == "DefaultLoading").AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading) ? 1.00m : loading;
            if (!decimal.TryParse(ruleAttributes.First(r => r.AttributeKey == wallType).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(wallType)}", "WallTypeCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The building security features calculation
        /// </summary>
        public Func<IEnumerable<SecurityFeatures>, IEnumerable<string>, IEnumerable<RuleTypeExtension>, decimal> BuildingSecurityFeaturesCalculation = (features, additions, extensions) =>
        {
            var objs = new List<dynamic>();
            var exts = extensions as IList<RuleTypeExtension> ?? extensions.ToList();
            foreach (var rule in exts.Where(e => e.Criteria != "Default").Select(e => e))
            {
                var source = rule.Criteria;
                var addidx = source.IndexOf("^", StringComparison.InvariantCulture);
                var exclidx = source.IndexOf("|", StringComparison.InvariantCulture);
                var indices = new[] { addidx, exclidx, source.Length }.OrderBy(i => i);
                dynamic checkObj = new ExpandoObject();
                checkObj.Inclusions = addidx == -1 && exclidx == -1
                    ? source
                    : source.Substring(0, addidx == -1 || addidx > exclidx ? exclidx == -1 ? addidx : exclidx : exclidx > addidx ? addidx == -1 ? exclidx : addidx : source.Length - 1);
                checkObj.Exclusions = exclidx == -1
                    ? string.Empty
                    : source.Substring(exclidx + 1, (indices.FirstOrDefault(i => i > 0 && i > exclidx) - exclidx - 1));
                checkObj.Additions = addidx == -1
                    ? string.Empty
                    : source.Substring(addidx + 1, (indices.FirstOrDefault(i => i > 0 && i > addidx) - addidx - 1));
                checkObj.Loading = rule.CriteriaFactors;
                objs.Add(checkObj);
            }

            if (features != null)
            {
                var feats = (features as IList<SecurityFeatures> ?? features.ToList()).Select(f => Enum.GetName(typeof(SecurityFeatures), f)).ToList();
                var adds = additions != null ? (additions as IList<string> ?? additions.ToList()) : new List<string>();
                foreach (var o in objs)
                {
                    var match = feats.ContainsAll(((string) o.Inclusions).Split(',')) && !feats.ContainsAll(((string) o.Exclusions).Split(',')) && adds.ContainsAll(((string) o.Additions).Split(','));
                    if (match) return (decimal) o.Loading;
                }
            }
            var loading = exts.Where(e => e.Criteria == "DefaultLoading").Select(e => e.CriteriaFactors).First();
            if (loading == 0.0m) throw new ArgumentOutOfRangeException($"{nameof(features)}", "BuildingSecurityFeaturesCalculation arguments violates rule constraints");
            return loading;
        };

        /// <summary>
        /// The subsidence landslip cover calculation
        /// </summary>
        public Func<bool, IEnumerable<RuleAttribute>, decimal> SubsidenceLandslipCoverCalculation = (hasCover, rules) =>
        {
            decimal loading;
            if (!decimal.TryParse(rules.First(r => r.AttributeKey == (hasCover ? "true" : "false")).AttributeValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out loading))
                throw new ArgumentOutOfRangeException($"{nameof(hasCover)}", "SubsidenceLandslipCoverCalculation arguments violates rule constraints");
            return loading;
        };
    }
}
