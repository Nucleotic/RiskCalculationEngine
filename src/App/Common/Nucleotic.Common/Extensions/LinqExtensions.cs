using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nucleotic.Common.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// A LINQ Expression tree to handle between clauses in a LINQ statement
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="low">The low.</param>
        /// <param name="high">The high.</param>
        /// <returns></returns>
        /// <remarks>Usage Example: var query = db.People.Between(person =&gt; person.Age, 18, 21);</remarks>
        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, TKey low, TKey high) where TKey : IComparable<TKey>
        {
            Expression key = Expression.Invoke(keySelector, keySelector.Parameters.ToArray());
            Expression lowerBound = Expression.GreaterThanOrEqual(key, Expression.Constant(low));
            Expression upperBound = Expression.LessThanOrEqual(key, Expression.Constant(high));
            Expression and = Expression.AndAlso(lowerBound, upperBound); Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(and, keySelector.Parameters);
            return source.Where(lambda);
        }

        /// <summary>
        /// Groups the while.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <remarks>Usage Example: GroupWhile((previous, current) => previous.Date.AddDays(1) == current.Date && previous.Price == current.Price)</remarks>
        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> predicate)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext()) yield break;
                var list = new List<T>() { iterator.Current };
                var previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    if (predicate(previous, iterator.Current))
                    {
                        list.Add(iterator.Current);
                    }
                    else
                    {
                        yield return list;
                        list = new List<T>() { iterator.Current };
                    }
                    previous = iterator.Current;
                }
                yield return list;
            }
        }

        /// <summary>
        /// Returns the First element or a default of null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static T? FirstOrNull<T>(this IEnumerable<T> items) where T : struct
        {
            foreach (var item in items)
                return item;
            return null;
        }

        /// <summary>
        /// Determines whether the specified source contains any of the specified elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>
        ///   <c>true</c> if the specified elements contains any; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> elements)
        {
            return source.Any(elements.Contains);
        }

        /// <summary>
        /// Determines whether the specified elements contains all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>
        ///   <c>true</c> if the specified elements contains all; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> elements)
        {
            return source.All(elements.Contains);
        }
    }
}