using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Nucleotic.DataContracts.CalculationEngine.Model.ModelExtensions
{
    public static class EntityExtensions
    {
        public static TEntity FirstOrDefaultCache<TEntity>(this DbSet<TEntity> queryable, Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            return queryable.Local.FirstOrDefault(condition.Compile()) // find in local cache
                   ?? queryable.FirstOrDefault(condition); // if local cache returns null check the db
        }

        public static TEntity SingleOrDefaultCache<TEntity>(this DbSet<TEntity> queryable, Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            return queryable.Local.SingleOrDefault(condition.Compile()) // find in local cache
                   ?? queryable.SingleOrDefault(condition); // if local cache returns null check the db
        }
    }
}