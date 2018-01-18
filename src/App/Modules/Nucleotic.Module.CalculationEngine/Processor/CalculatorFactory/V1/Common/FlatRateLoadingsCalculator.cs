using System;
using System.Collections.Generic;
using System.Globalization;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;
using System.Linq;
using Nucleotic.DataContracts.CalculationEngine;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V1.Common
{
    public class FlatRateLoadingsCalculator : IFlatRateLoadingsCalculator
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public LoadingsContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatRateLoadingsCalculator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public FlatRateLoadingsCalculator(LoadingsContext context)
        {

            Context = context;
        }

        /// <summary>
        /// Calculates the flat rate loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateFlatRateLoading(Func<IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(Context.Attributes.Where(a => a.AttributeName == Enum.GetName(typeof(FlatRateCalculationTypes), Context.FlatRateCalculationType)));
            return loading;
        }

        /// <summary>
        /// Calculates the flat rate loading.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateFlatRateLoading(Func<decimal, string, IEnumerable<BandedRuleAttribute>, decimal> calculator)
        {
            var loading = calculator(Context.BasicSumInsured, Context.ProductName, Context.ExtendedBandedAttributes);
            return loading;
        }

        /// <summary>
        /// Calculates the minimum amount.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public decimal CalculateMinimumAmount(Func<IEnumerable<RuleAttribute>, decimal> calculator)
        {
            var loading = calculator(Context.Attributes.Where(a => a.AttributeName == Enum.GetName(typeof(FlatRateCalculationTypes), Context.FlatRateCalculationType)));
            return loading;
        }

        /// <summary>
        /// Calculates the flat rate premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateFlatRateMonthlyPremium(Func<decimal, IEnumerable<AssetLoading>, decimal> calculator)
        {
            var premium = calculator(Context.BasicSumInsured, Context.Loadings);
            return premium;
        }

        /// <summary>
        /// Calculates the base rate annual premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        public decimal CalculateBaseRateAnnualPremium(Func<decimal, decimal, decimal, int, decimal> calculator)
        {
            var minimum = decimal.Parse(Context.ExtendedBandedAttributes.First().AttributeValue, CultureInfo.InvariantCulture);
            var premium = calculator(Context.BasicSumInsured, Context.BaseRate, minimum, Context.ProductCount);
            return premium;
        }

        /// <summary>
        /// Calculates the product base premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public decimal CalculateProductBasePremium(Func<string, IEnumerable<ExtendedRuleAttribute>, decimal> calculator)
        {
            var basePremium = calculator(Context.ProductName, Context.ExtendedAttributes);
            return basePremium;
        }

        /// <summary>
        /// Calculates the product specific flat rate premium.
        /// </summary>
        /// <param name="calculator">The calculator.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public decimal CalculateProductMontlyPremium(Func<decimal, int, decimal> calculator)
        {
            var premium = calculator(Context.BaseRate, Context.ProductCount);
            return premium;
        }
    }
}
