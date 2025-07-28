using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace SalesSystem.Shared.Extensions;

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

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IDictionary<string, string> filters)
    {
        if (filters == null || !filters.Any()) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var predicateList = new List<Expression>();

        foreach (var kv in filters)
        {
            var key = kv.Key;
            var value = kv.Value;

            var prop = typeof(T).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) continue;

            var member = Expression.PropertyOrField(parameter, prop.Name);
            Expression comparison;

            if (prop.PropertyType == typeof(string))
            {
                if (value.StartsWith("*") && value.EndsWith("*"))
                    comparison = Expression.Call(member, nameof(string.Contains), null, Expression.Constant(value.Trim('*')));
                else if (value.StartsWith("*"))
                    comparison = Expression.Call(member, nameof(string.EndsWith), null, Expression.Constant(value.TrimStart('*')));
                else if (value.EndsWith("*"))
                    comparison = Expression.Call(member, nameof(string.StartsWith), null, Expression.Constant(value.TrimEnd('*')));
                else
                    comparison = Expression.Equal(member, Expression.Constant(value));
            }
            else
            {
                var converted = Convert.ChangeType(value, prop.PropertyType);
                comparison = Expression.Equal(member, Expression.Constant(converted));
            }

            predicateList.Add(comparison);
        }

        if (!predicateList.Any()) return query;

        var body = predicateList.Aggregate(Expression.AndAlso);
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        return query.Where(lambda);
    }
    
    public static Dictionary<string, string> GetFilters(this IQueryCollection queryCollection)
    {
        var dictionary = queryCollection.Where(q => !q.Key.StartsWith("_"))
            .ToDictionary(q => q.Key, q => q.Value.ToString());

        return dictionary;
    }
    
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int size)
    {
        return query
            .Skip((page - 1) * size)
            .Take(size);
    }
}