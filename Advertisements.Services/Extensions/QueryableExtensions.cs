using Advertisements.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Advertisements.Services.Extensions
{    
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenByDescending", propertyName, comparer);
        }

        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> query, string propertyName, bool? desc, IComparer<object> comparer = null)
        {
            if (!desc.HasValue) desc = false;
            var order = desc.Value ? "OrderByDescending" : "OrderBy";

            return CallOrderedQueryable(query, order, propertyName, comparer);
        }


        public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName, IComparer<object> comparer = null)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer != null
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                    )
                )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                    )
                );
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int? skip, int? take)
        {
            if (skip.HasValue)
                query = query.Skip(skip.Value);
            if (take.HasValue)
                query = query.Take(take.Value);
            return query;
        }

        /// <summary>
        /// Добавляет к переменной класса IQueryable<T> условие Where() для поиска значений 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchFields"></param>
        /// <returns></returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> query, List<SearchRequest> searchFields)
        {
            //Создаем выражение вида x => x.propertyName.ToString().Contains(search)
            if (searchFields.Count == 0)
                return query;

            //Часть выражения которое представляет элемент коллекции
            var param = Expression.Parameter(typeof(T), "x");

            Expression body = Expression.Constant(true);
            foreach (var search in searchFields)
            {
                var bodyContains = GetSearchExpression(search, typeof(T), param);
                body = Expression.AndAlso(bodyContains, body);
            }

            var resultQuery = (IQueryable<T>)query.Provider.CreateQuery(
                Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { query.ElementType },
                query.Expression,
                Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[] { param })));

            return resultQuery;
        }

        /// <summary>
        /// Возвращает выражение для поиска, в зависимости от того какие поля модели SearchRequest заполнены
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Expression GetSearchExpression(SearchRequest request, Type type, Expression param)
        {
            if (request.SearchValue != null)
            {
                //Часть выражения которое представляет элемент коллекции
                //var param = Expression.Parameter(type, "x");
                //Часть выражения которое представляет искомое значение
                var searchValue = Expression.Constant(request.SearchValue);
                //Поле для поиска
                var property = type.GetProperty(request.FieldName);
                //Указываем поле для поиска
                var paramField = Expression.MakeMemberAccess(param, property);
                //Добавляем вызов ToString()
                Expression bodyToString;
                if (property.PropertyType == typeof(string))
                    bodyToString = paramField;
                else
                    bodyToString = Expression.Call(paramField, typeof(Object).GetMethod("ToString"));
                //Добавляем вызов Contains()
                var bodyContains = Expression.Call(bodyToString, typeof(string).GetMethod("Contains", new[] { typeof(string) }), searchValue);

                return bodyContains;
            }
            if (request.SearchFrom != null && request.SearchTo != null)
            {
                //Часть выражения которое представляет элемент коллекции
                //var param = Expression.Parameter(type, "x");
                //Поле для поиска
                var property = type.GetProperty(request.FieldName);

                //Часть выражения которое представляет начало искомого диапазона
                var searchFrom = Expression.Constant(Convert.ChangeType(request.SearchFrom, property.PropertyType));
                //Часть выражения которое представляет конец искомого диапазона
                var searchTo = Expression.Constant(Convert.ChangeType(request.SearchTo, property.PropertyType));
                
                //Указываем поле для поиска
                var paramField = Expression.MakeMemberAccess(param, property);
                //Выражение условия "больше либо равно"
                var bodyGreaterThan = Expression.GreaterThanOrEqual(paramField, searchFrom);
                //Выражение условия "Меньше либо равно"
                var bodyLessThan = Expression.LessThanOrEqual(paramField, searchTo);

                return Expression.AndAlso(bodyGreaterThan, bodyLessThan);
            }

            return Expression.Constant(true);
        }
    }
}
