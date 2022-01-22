using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GringottsBank.BLL.Extensions
{
    public static class EntityFilterExtensions
    {
        public static Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity, TFilter>(this TFilter filter)
        {
            Expression<Func<TEntity, bool>> whereExpression = null;
            if (filter != null)
            {
                var entityProperties = typeof(TEntity).GetProperties();
                var properties = filter.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var name = property.Name;
                    var value = property.GetValue(filter, null);
                    if (value != null)
                    {
                        (var finalName, var entityProperty) = GetEntityProperty(name, property, entityProperties);
                        if (entityProperty != null)
                        {
                            var param = Expression.Parameter(typeof(TEntity), "x");
                            var expression = Expression.Lambda<Func<TEntity, bool>>(
                                Expression.Equal(
                                    CreatePropertyExpression(param, finalName),
                                    Expression.Constant(value)
                                ),
                                param
                            );
                            //Expression<Func<Animal, bool>> expression = x => animalProperty.GetValue(x, null) == value;
                            whereExpression = whereExpression == null ? expression : whereExpression.And(expression);
                        }
                        else if (finalName.EndsWith("Start") || finalName.EndsWith("End"))
                        {
                            bool isStart = finalName.EndsWith("Start");
                            int removeCount = isStart ? 5 : 3;
                            var newName = finalName.Remove(finalName.Length - removeCount, removeCount);
                            (finalName, entityProperty) = GetEntityProperty(newName, property, entityProperties);
                            if (entityProperty != null)
                            {
                                var param = Expression.Parameter(typeof(TEntity), "x");
                                var innerExpression = isStart ?
                                        Expression.GreaterThanOrEqual(
                                            CreatePropertyExpression(param, finalName),
                                            Expression.Constant(value)
                                    ) : Expression.LessThanOrEqual(
                                            CreatePropertyExpression(param, finalName),
                                            Expression.Constant(value)
                                    );
                                var expression = Expression.Lambda<Func<TEntity, bool>>(
                                    innerExpression,
                                    param
                                );
                                whereExpression = whereExpression == null ? expression : whereExpression.And(expression);
                            }
                        }
                    }
                }
            }
            return whereExpression;
        }

        private static (string, PropertyInfo) GetEntityProperty(string name, PropertyInfo filterProperty, PropertyInfo[] entityProperties)
        {
            PropertyInfo entityProperty = null;
            var displayNameAttribute = (DisplayNameAttribute)filterProperty.GetCustomAttribute(typeof(DisplayNameAttribute));
            if (displayNameAttribute != null)
            {
                name = displayNameAttribute.DisplayName;
                var names = name.Split('.');
                entityProperty = GetInnerProperty(entityProperties, names);
            }
            else
            {
                entityProperty = entityProperties.FirstOrDefault(x => x.Name == name);
            }
            return (name, entityProperty);
        }

        private static PropertyInfo GetInnerProperty(PropertyInfo[] properties, string[] names)
        {
            if (names.Length > 1)
            {
                var tagetProperty = properties.FirstOrDefault(x => x.Name == names[0]);
                var newProperties = tagetProperty.PropertyType.GetProperties();
                names = names.Skip(1).ToArray();
                return GetInnerProperty(newProperties, names);
            }
            else
            {
                return properties.FirstOrDefault(x => x.Name == names[0]);
            }
        }

        static Expression CreatePropertyExpression(ParameterExpression param, string propertyName)
        {
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.Property(body, member);
            }
            return body;
        }
    }
}
