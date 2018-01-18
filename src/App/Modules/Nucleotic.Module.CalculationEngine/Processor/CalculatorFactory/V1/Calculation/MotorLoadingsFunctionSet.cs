using Nucleotic.Common.Extensions;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Calculation
{
    public class MotorLoadingsFunctionSet
    {
        /// <summary>
        /// The vehicle use loading calculation
        /// </summary>
        public Func<ScaleOfUse, IEnumerable<RuleAttribute>, decimal> VehicleUseLoadingCalculation = (use, rules) =>
        {
            var ruleAttributes = rules as IList<RuleAttribute> ?? rules.ToList();
            try
            {
                var loading = ruleAttributes.Where(r => r.AttributeKey == use.ToString()).Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).First();
                return loading;
            }
            catch (Exception)
            {
                return ruleAttributes.Where(r => r.AttributeKey == "PrivateAndBusiness").Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).First(); //default
            }
        };

        /// <summary>
        /// The vehicle make model loading calculation
        /// </summary>
        public Func<string, string, bool, IEnumerable<VehicleLoading>, IEnumerable<RuleAttribute>, decimal> VehicleMakeModelLoadingCalculation = (make, model, isHighPerformance, vehicles, attributes) =>
        {
            var hpRule = attributes.First(a => a.AttributeKey == "HighPerformance");
            var defaultLoading = decimal.Parse(attributes.First(a => a.AttributeName == "VehicleLoadings" && a.AttributeKey == "DefaultLoading").AttributeValue, CultureInfo.InvariantCulture);
            var vehicleList = vehicles.ToList();
            var makeLoading = vehicleList.Where(v => v.VehicleMake == make).Select(v => v.LoadingRate).FirstOrNull();
            if (makeLoading == null) return !isHighPerformance ? defaultLoading : defaultLoading * decimal.Parse(hpRule.AttributeValue, CultureInfo.InvariantCulture);
            var modelLoading = vehicleList.Where(v => v.VehicleMake == make && v.VehicleModel == model).Select(v => (decimal)v.LoadingRate).FirstOrNull();
            if (modelLoading == null) return !isHighPerformance
                ? makeLoading.HasValue ? Math.Max(defaultLoading, makeLoading.Value) : defaultLoading
                : 1.00m * decimal.Parse(hpRule.AttributeValue, CultureInfo.InvariantCulture);
            return !isHighPerformance ? Math.Max(makeLoading.Value, modelLoading.Value) : Math.Max(makeLoading.Value, modelLoading.Value) * decimal.Parse(hpRule.AttributeValue, CultureInfo.InvariantCulture);
        };

        /// <summary>
        /// The vehicle type loading calculation
        /// </summary>
        public Func<VehicleType, IEnumerable<RuleAttribute>, decimal> VehicleTypeLoadingCalculation = (vehicleType, rules) =>
        {
            var loading = rules.Where(r => r.AttributeKey == vehicleType.ToString()).Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).FirstOrNull();
            if (loading == null) throw new ArgumentOutOfRangeException($"{nameof(vehicleType)}", "VehicleTypeLoadingCalculation arguments violates rule constraints");
            return loading.Value;
        };

        /// <summary>
        /// The vehicle tracker loading calculation
        /// </summary>
        public Func<bool, IEnumerable<RuleAttribute>, decimal> VehicleTrackerLoadingCalculation = (hasTracker, rules) =>
        {
            return hasTracker
                ? rules.Where(r => r.AttributeKey == "HasTracker").Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).First()
                : rules.Where(r => r.AttributeKey == "NoTracker").Select(r => decimal.Parse(r.AttributeValue, CultureInfo.InvariantCulture)).First();
        };
    }
}