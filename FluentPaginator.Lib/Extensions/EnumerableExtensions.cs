using System;
using System.Collections.Generic;
using System.Linq;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Extensions;

/// <summary>
/// Extensions for IEnumerable
/// If you are using EF Core, you should use the <see cref="QueryableExtensions"/> instead
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Paginate an IEnumerable
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static Page<T> Paginate<T, TKey>(this IEnumerable<T> self, PaginationParameter paginationParameter,
        Func<T, TKey>? orderFunc = null)
    {
        return new Paginator<T>(self.AsQueryable()).Paginate(paginationParameter, orderFunc);
    }

    /// <summary>
    /// Paginate an IEnumerable with urls
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The url pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static UrlPage<T> UrlPaginate<T, TKey>(this IEnumerable<T> self, UrlPaginationParameter paginationParameter,
        Func<T, TKey>? orderFunc = null)
    {
        return new UrlPaginator<T>(self.AsQueryable()).Paginate(paginationParameter, orderFunc);
    }

    /// <summary>
    /// Paginate an IEnumerable using cursor pagination
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The pagination param</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <returns>The paginated result</returns>
    public static Page<T> CursorPaginate<T>(this IEnumerable<T> self, PaginationParameter paginationParameter)
    {
        return new CursorPaginator<T>(self.AsQueryable()).Paginate<T>(paginationParameter);
    }
}