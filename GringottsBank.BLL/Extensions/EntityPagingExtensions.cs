using GringottsBank.Core.Concrete;
using GringottsBank.Entities.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GringottsBank.BLL.Extensions
{
    public static class EntityPagingExtensions
    {
        public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> CreateOrderByExpression<TEntity>(this PagingModel paging)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null;
            if (paging != null)
            {
                var entityProperties = typeof(TEntity).GetProperties();
                if (!string.IsNullOrEmpty(paging.OrderBy))
                {
                    var name = paging.OrderBy;
                    var methodName = paging.OrderDirection == "desc" ? "OrderByDescending" : "OrderBy";
                    var entityProperty = entityProperties.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                    if (entityProperty != null)
                    {
                        Type type = entityProperty.PropertyType;
                        var outerParam = Expression.Parameter(typeof(IQueryable<TEntity>), "p");
                        var outerExpression = Expression.Lambda(outerParam, outerParam);

                        var param = Expression.Parameter(typeof(TEntity), "x");
                        LambdaExpression lambda = Expression.Lambda(
                                Expression.Property(param, entityProperty.Name),
                            param
                        );
                        var resultExpression = Expression.Call(typeof(Queryable),
                            methodName,
                            new Type[] { typeof(TEntity), type },
                            outerExpression.Body,
                            Expression.Quote(lambda));
                        orderBy = (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)Expression.Lambda(resultExpression, outerParam).Compile();
                    }
                }
            }
            return orderBy;
        }

        public static EntityPaging CreatePaging(this PagingModel paging)
        {
            EntityPaging entityPaging = null;
            if (paging != null)
            {
                entityPaging = new EntityPaging();
                entityPaging.PageNumber = paging.PageNumber;
                entityPaging.ItemCount = paging.ItemCount;
            }
            return entityPaging;
        }
    }
}
