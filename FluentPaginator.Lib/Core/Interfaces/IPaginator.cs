using System;
using System.Linq.Expressions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Core.Interfaces;

/// <summary>
/// Default interface for the Paginator and the CursorPaginator
/// </summary>
/// <typeparam name="T">The type that will be in the pagination data</typeparam>
public interface IPaginator<T>
{
    /// <summary>
    /// Paginate a source of data
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size and the current page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    public Page<T> Paginate<TKey>(PaginationParameter paginationParameter, Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending);
}