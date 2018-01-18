using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using System;
using System.Linq;
using Nucleotic.Framework.Engine.Rules;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting
{
    public class ValidateWaiverExclusionsCommand : AssemblorCommand<ExcessWaiverContext>
    {
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
        public override void Assemble(ExcessWaiverContext assembly)
        {
            if (assembly.PolicyRiskItemRatingType != RiskItemRatingType.MotorVehicle) return;
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
                    var compiledRule = rulesEngine.CompileRule<ExcessWaiverContext>(rule);
                    if (compiledRule(assembly)) continue;
                    assembly.IsDeclinedReferred = true;
                    assembly.DeclinedReferredReason = rule.Reason;
                    assembly.CalculatedAnnualPremium = assembly.BasicSumInsured;
                    assembly.CalculatedMonthlyPremium = assembly.BasicSumInsured / 12;
                }
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage($"{nameof(ValidateWaiverExclusionsCommand)} : Assemble", ex);
                assembly.Errors.Add(ex);
                throw;
            }
        }
    }
}