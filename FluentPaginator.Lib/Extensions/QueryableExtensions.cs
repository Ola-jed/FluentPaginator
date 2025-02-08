using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Extensions;

/// <summary>
///     Extensions for IQueryable
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    ///     Paginate an IQueryable
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
    ///     Paginate an IQueryable asynchronously
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static Task<Page<T>> AsyncPaginate<T, TKey>(this IQueryable<T> self, PaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null, PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CancellationToken cancellationToken = default)
    {
        return new Paginator<T>(self).AsyncPaginate(paginationParameter, orderFunc, paginationOrder, cancellationToken);
    }

    /// <summary>
    ///     Paginate an IQueryable with urls
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

    /// <summary>
    ///     Paginate an IQueryable with urls
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The url pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static Task<UrlPage<T>> AsyncUrlPaginate<T, TKey>(this IQueryable<T> self,
        UrlPaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null, PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CancellationToken cancellationToken = default)
    {
        return new UrlPaginator<T>(self).AsyncPaginate(paginationParameter, orderFunc, paginationOrder,
            cancellationToken);
    }

    /// <summary>
    ///     Paginate an IQueryable, using a cursor
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The parameter for page size</param>
    /// <param name="firstKey">The starting item for the page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static CursorPage<T> Paginate<T, TKey>(
        this IQueryable<T> self,
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending
    ) where TKey : struct, IComparable<TKey>
    {
        return new CursorPaginator<T>(self)
            .Paginate(paginationParameter, firstKey, orderFunc, paginationOrder);
    }

    /// <summary>
    ///     Paginate an IQueryable asynchronously, using a cursor
    /// </summary>
    /// <param name="self">The IQueryable to paginate</param>
    /// <param name="paginationParameter">The parameter for page size</param>
    /// <param name="firstKey">The starting item for the page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static Task<CursorPage<T>> AsyncPaginate<T, TKey>(
        this IQueryable<T> self,
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CancellationToken cancellationToken = default
    ) where TKey : struct, IComparable<TKey>
    {
        return new CursorPaginator<T>(self)
            .AsyncPaginate(paginationParameter, firstKey, orderFunc, paginationOrder, cancellationToken);
    }
}