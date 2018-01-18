using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;
using System.Globalization;
using System.Linq;
using Nucleotic.Framework.Engine.Rules;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class ValidateLoadingExclusionsCommand : AssemblorCommand<LoadingsContext>
    {
        private decimal _;

        /// <summary>
        /// Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <example>
        ///     rule.Operator = ExpressionType.AndAlso.ToString("g");
        ///     rule.Rules = new List<Rule>();
        ///     rule.Rules.AddRange(from exclusion in assembly.UnderwritingExclusions
        ///         select new Rule()
        ///         {
        ///             MemberName = exclusion.MemberName,
        ///             Operator = ((ExpressionType)Enum.Parse(typeof(ExpressionType), exclusion.Operator)).ToString("g"),
        ///             TargetValue = exclusion.ExclusionValue,
        ///             Reason = exclusion.Reason
        ///         });
        /// </example>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                var rulesEngine = new ExpressionRuleEngine();
                if (!assembly.UnderwritingExclusions.Any()) return;
                foreach (var exclusion in assembly.UnderwritingExclusions)
                {
                    var rule = new Rule
                    {
                        MemberName = exclusion.MemberName,
                        Operator = exclusion.Operator,
                        TargetValue = exclusion.ExclusionValue,
                        Reason = exclusion.Reason
                    };

                    //If rule comparison is a number rule do this
                    //Regex.IsMatch(rule.TargetValue, @"^\d+$", RegexOptions.CultureInvariant)
                    if (decimal.TryParse(rule.TargetValue, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    {
                        var compiledRule = rulesEngine.CompileRule<LoadingsContext>(rule);
                        if (compiledRule(assembly)) continue;
                    }
                    else
                    {
                        var stringRule = rulesEngine.CompileRule<LoadingsContext>(rule, new[] {rule.TargetValue});
                        if (stringRule(assembly)) continue;
                    }

                    assembly.IsDeclinedReferred = true;
                    assembly.DeclinedReferredReason = rule.Reason;
                    assembly.CalculatedAnnualPremium = assembly.BasicSumInsured;
                    assembly.CalculatedMonthlyPremium = assembly.BasicSumInsured / 12;
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(ValidateLoadingExclusionsCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
                throw;
            }
        }
    }
}