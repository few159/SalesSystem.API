using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query;

        var orders = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
        bool first = true;

        foreach (var order in orders)
        {
            var parts = order.Trim().Split(' ');
            var property = parts[0].Trim();
            var descending = parts.Length > 1 && parts[1].Trim().Equals("desc", StringComparison.OrdinalIgnoreCase);

            query = ApplyOrder(query, property, descending, first);
            first = false;
        }

        return query;
    }

    private static IQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, bool descending, bool first)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = property.Split('.').Aggregate((Expression)param, Expression.PropertyOrField);
        var lambda = Expression.Lambda(body, param);

        string methodName = first
            ? (descending ? "OrderByDescending" : "OrderBy")
            : (descending ? "ThenByDescending" : "ThenBy");

        var result = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), body.Type)
            .Invoke(null, new object[] { source, lambda });

        return (IQueryable<T>)result!;
    }

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IQueryCollection filters)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var predicateList = new List<Expression>();

        foreach (var filter in filters)
        {
            string key = filter.Key;
            string value = filter.Value;

            if (key.StartsWith("_")) continue; // ignora _page, _size, _order, _minX, _maxX

            var prop = typeof(T).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) continue;

            var member = Expression.PropertyOrField(parameter, prop.Name);
            Expression comparison;

            if (prop.PropertyType == typeof(string))
            {
                if (value.StartsWith("*") && value.EndsWith("*"))
                {
                    var contains = value.Trim('*');
                    comparison = Expression.Call(member, nameof(string.Contains), null, Expression.Constant(contains));
                }
                else if (value.StartsWith("*"))
                {
                    var endsWith = value.TrimStart('*');
                    comparison = Expression.Call(member, nameof(string.EndsWith), null, Expression.Constant(endsWith));
                }
                else if (value.EndsWith("*"))
                {
                    var startsWith = value.TrimEnd('*');
                    comparison = Expression.Call(member, nameof(string.StartsWith), null, Expression.Constant(startsWith));
                }
                else
                {
                    comparison = Expression.Equal(member, Expression.Constant(value));
                }
            }
            else
            {
                var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                comparison = Expression.Equal(member, Expression.Constant(convertedValue));
            }

            predicateList.Add(comparison);
        }

        // Range filters (_minPrice, _maxPrice etc.)
        foreach (var kv in filters)
        {
            var key = kv.Key;
            var value = kv.Value;

            if (key.StartsWith("_min"))
            {
                var propName = key.Substring(4);
                var prop = typeof(T).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;

                var member = Expression.PropertyOrField(parameter, prop.Name);
                var constant = Expression.Constant(Convert.ChangeType(value.ToString(), prop.PropertyType));
                predicateList.Add(Expression.GreaterThanOrEqual(member, constant));
            }
            else if (key.StartsWith("_max"))
            {
                var propName = key.Substring(4);
                var prop = typeof(T).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;

                var member = Expression.PropertyOrField(parameter, prop.Name);
                var constant = Expression.Constant(Convert.ChangeType(value.ToString(), prop.PropertyType));
                predicateList.Add(Expression.LessThanOrEqual(member, constant));
            }
        }

        if (predicateList.Count == 0)
            return query;

        var combined = predicateList.Aggregate(Expression.AndAlso);
        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);

        return query.Where(lambda);
    }
}