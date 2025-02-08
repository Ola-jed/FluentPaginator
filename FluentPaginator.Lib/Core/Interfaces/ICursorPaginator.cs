using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Core.Interfaces;

/// <summary>
///     Default interface for the CursorPaginator
/// </summary>
/// <typeparam name="T">The type that will be in the pagination data</typeparam>
public interface ICursorPaginator<T>
{
    /// <summary>
    ///     Paginate a source of data
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size</param>
    /// <param name="firstKey">The starting item for the page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    public CursorPage<T> Paginate<TKey>(
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending
    ) where TKey : struct, IComparable<TKey>;

    /// <summary>
    ///     Paginate a source of data asynchronously, using Entity Framework extensions
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size</param>
    /// <param name="firstKey">The starting item for the page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    public Task<CursorPage<T>> AsyncPaginate<TKey>(
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CancellationToken cancellationToken = default
    ) where TKey : struct, IComparable<TKey>;
}