using System;
using System.Linq;
using System.Linq.Expressions;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Parameter;
using FluentPaginator.Lib.Page;

namespace FluentPaginator.Lib.Extensions;

/// <summary>
/// Extensions for IQueryable
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Paginate an IQueryable
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static Page<T> Paginate<T, TKey>(this IQueryable<T> self, PaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null, PaginationOrder paginationOrder = PaginationOrder.Ascending)
    {
        return new Paginator<T>(self).Paginate(paginationParameter, orderFunc, paginationOrder);
    }

    /// <summary>
    /// Paginate an IQueryable with urls
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The url pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static UrlPage<T> UrlPaginate<T, TKey>(this IQueryable<T> self, UrlPaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null, PaginationOrder paginationOrder = PaginationOrder.Ascending)
    {
        return new UrlPaginator<T>(self).Paginate(paginationParameter, orderFunc, paginationOrder);
    }
}