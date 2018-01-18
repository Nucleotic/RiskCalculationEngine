using System;
using System.Linq;
using System.Linq.Expressions;

namespace Nucleotic.Framework.Engine.Rules
{
    public partial class ExpressionRuleEngine
    {
        /// <summary>
        /// Compiles the rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r">The r.</param>
        /// <param name="arr">The arr.</param>
        /// <returns></returns>
        public Func<T, bool> CompileRule<T>(Rule r, string[] arr)
        {
            var queryableData = arr.AsQueryable();
            var pe = Expression.Parameter(typeof(string), r.MemberName);
            Expression right = Expression.Constant(r.TargetValue);
            Expression left = Expression.Call(pe, typeof(string).GetMethod("Contains"), right);
            var call = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] {queryableData.ElementType},
                queryableData.Expression,
                Expression.Lambda<Func<string, bool>>(left, pe));

            var res = queryableData.Provider.CreateQuery<string>(call);
            //var p = Expression.LeftShiftAssign(left, right);
            var paramUser = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>(res.Expression, paramUser).Compile();
        }
    }
}
