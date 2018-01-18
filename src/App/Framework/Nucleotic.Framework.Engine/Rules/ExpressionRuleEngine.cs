using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Nucleotic.Framework.Engine.Rules
{
    /// <summary>
    /// LINQ dynamic compilation Binary-Expression-Tree based rule engine
    /// </summary>
    public partial class ExpressionRuleEngine
    {
        private readonly ExpressionType[] _operators = { ExpressionType.And, ExpressionType.AndAlso, ExpressionType.Or, ExpressionType.OrElse, ExpressionType.Equal,
            ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan, ExpressionType.LessThanOrEqual };

        /// <summary>
        /// Compiles the rule.
        /// </summary>
        /// <typeparam name="T">The object used for the rule execution</typeparam>
        /// <param name="r">The rule applied to the object</param>
        /// <returns></returns>
        public Func<T, bool> CompileRule<T>(Rule r)
        {
            var paramUser = Expression.Parameter(typeof(T));
            var expr = GetExpressionForRule<T>(r, paramUser);
            return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
        }

        /// <summary>
        /// Compiles the rules.
        /// </summary>
        /// <typeparam name="T">The object used for the rule execution</typeparam>
        /// <param name="rules">The rules applied to the object.</param>
        /// <returns></returns>
        public Func<T, bool> CompileRules<T>(IList<Rule> rules)
        {
            var paramUser = Expression.Parameter(typeof(T));
            var expr = BuildNestedExpression<T>(rules, paramUser, ExpressionType.And);
            return Expression.Lambda<Func<T, bool>>(expr, paramUser).Compile();
        }

        /// <summary>
        /// Checks if the rule passes.
        /// </summary>
        /// <typeparam name="T">The object used for the rule execution</typeparam>
        /// <param name="rules">The rules applied to the object</param>
        /// <param name="toInspect">To inspect.</param>
        /// <returns></returns>
        public bool PassesRules<T>(IList<Rule> rules, T toInspect)
        {
            return CompileRules<T>(rules).Invoke(toInspect);
        }

        /// <summary>
        /// Builds the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r">The r.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        private static Expression BuildExpression<T>(Rule r, Expression param)
        {
            Expression propExpression;
            Type propType;

            ExpressionType tBinary;
            if (string.IsNullOrEmpty(r.MemberName))
            {
                propExpression = param;
                propType = propExpression.Type;
            }
            else if (r.MemberName.Contains('.'))
            {
                var childProperties = r.MemberName.Split('.');
                var property = typeof(T).GetProperty(childProperties[0]);
                //var paramExp = Expression.Parameter(typeof(T), "SomeObject");

                propExpression = Expression.PropertyOrField(param, childProperties[0]);
                for (var i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    if (property != null)
                        propExpression = Expression.PropertyOrField(propExpression, childProperties[i]);
                }
                propType = propExpression.Type;
            }
            else
            {
                propExpression = Expression.PropertyOrField(param, r.MemberName);
                propType = propExpression.Type;
            }

            // is the operator a known .NET operator?
            if (Enum.TryParse(r.Operator, out tBinary))
            {
                var right = StringToExpression(r.TargetValue, propType);
                return Expression.MakeBinary(tBinary, propExpression, right);
            }
            if (r.Operator == "IsMatch")
            {
                return Expression.Call(typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string), typeof(RegexOptions) }), propExpression,
                    Expression.Constant(r.TargetValue, typeof(string)), Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions)));
            }
            var inputs = r.Inputs.Select(x => x.GetType()).ToArray();
            var methodInfo = propType.GetMethod(r.Operator, inputs);
            if (!methodInfo.IsGenericMethod)
                inputs = null; //Only pass in type information to a Generic Method
            var expressions = r.Inputs.Select(Expression.Constant).ToArray();
            return Expression.Call(propExpression, r.Operator, inputs, expressions);
        }

        /// <summary>
        /// Gets the expression for rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r">The r.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        private Expression GetExpressionForRule<T>(Rule r, ParameterExpression param)
        {
            ExpressionType nestedOperator;
            if (Enum.TryParse(r.Operator, out nestedOperator) && _operators.Contains(nestedOperator) && r.Rules != null && r.Rules.Any())
                return BuildNestedExpression<T>(r.Rules, param, nestedOperator);
            return BuildExpression<T>(r, param);
        }

        /// <summary>
        /// Builds the nested expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rules">The rules.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private Expression BuildNestedExpression<T>(IEnumerable<Rule> rules, ParameterExpression param, ExpressionType operation)
        {
            var expressions = rules.Select(r => GetExpressionForRule<T>(r, param)).ToList();
            var expr = BinaryExpression(expressions, operation);
            return expr;
        }

        /// <summary>
        /// Returns the binary expression.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <returns></returns>
        private static Expression BinaryExpression(IList<Expression> expressions, ExpressionType operationType)
        {
            Func<Expression, Expression, Expression> methodExp = Expression.And;
            switch (operationType)
            {
                case ExpressionType.And:
                    methodExp = Expression.And;
                    break;
                case ExpressionType.Or:
                    methodExp = Expression.Or;
                    break;
                case ExpressionType.OrElse:
                    methodExp = Expression.OrElse;
                    break;

                case ExpressionType.AndAlso:
                    methodExp = Expression.AndAlso;
                    break;

                case ExpressionType.Equal:
                    methodExp = Expression.Equal;
                    break;

                case ExpressionType.GreaterThan:
                    methodExp = Expression.GreaterThan;
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    methodExp = Expression.GreaterThanOrEqual;
                    break;

                case ExpressionType.LessThanOrEqual:
                    methodExp = Expression.LessThanOrEqual;
                    break;
            }
            if (expressions.Count == 1)
                return expressions[0];
            var exp = methodExp(expressions[0], expressions[1]);
            for (var i = 2; expressions.Count > i; i++)
            {
                exp = methodExp(exp, expressions[i]);
            }
            return exp;
        }

        /// <summary>
        /// Returns the expression of a string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propType">Type of the property.</param>
        /// <returns></returns>
        private static Expression StringToExpression(string value, Type propType)
        {
            var right = Expression.Constant(value.ToLower() == "null" ? null : Convert.ChangeType(value, propType, CultureInfo.InvariantCulture));
            return right;
        }

        /// <summary>
        /// Returns the And expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns></returns>
        private Expression AndExpressions(IList<Expression> expressions)
        {
            if (expressions.Count == 1)
                return expressions[0];
            Expression exp = Expression.And(expressions[0], expressions[1]);
            for (var i = 2; expressions.Count > i; i++)
            {
                exp = Expression.And(exp, expressions[i]);
            }
            return exp;
        }

        /// <summary>
        /// Returns the Or expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns></returns>
        private Expression OrExpressions(IList<Expression> expressions)
        {
            if (expressions.Count == 1)
                return expressions[0];
            Expression exp = Expression.Or(expressions[0], expressions[1]);
            for (var i = 2; expressions.Count > i; i++)
            {
                exp = Expression.Or(exp, expressions[i]);
            }
            return exp;
        }
    }
}