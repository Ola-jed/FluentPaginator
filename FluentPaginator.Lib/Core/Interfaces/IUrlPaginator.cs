using System;
using System.Linq.Expressions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Core.Interfaces;

/// <summary>
/// The default interface for paginating data with urls
/// </summary>
/// <typeparam name="T">The type that will be in the pagination data</typeparam>
public interface IUrlPaginator<T>
{
    /// <summary>
    /// Paginate a source of data with the urls
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size and the current page and the base url</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    UrlPage<T> Paginate<TKey>(UrlPaginationParameter paginationParameter, Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending);
}