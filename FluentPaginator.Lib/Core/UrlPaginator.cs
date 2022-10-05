using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentPaginator.Lib.Core.Interfaces;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Core;

/// <summary>
/// The Url Paginator for web apps
/// </summary>
/// <typeparam name="T">The type that will be in the pagination data</typeparam>
public class UrlPaginator<T> : IUrlPaginator<T>
{
    private const char AmpersandSign = '&';
    private const char QuerySign = '?';
    private const char EqualsSign = '=';
    
    private readonly IQueryable<T> _source;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="source">The IQueryable used as source data</param>
    public UrlPaginator(IQueryable<T> source)
    {
        _source = source;
    }

    /// <summary>
    /// Paginate a source of data with the urls
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size and the current page and the base url</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    public UrlPage<T> Paginate<TKey>(UrlPaginationParameter paginationParameter, Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending)
    {
        IEnumerable<T> items;
        var toSkip = (paginationParameter.PageNumber - 1) * paginationParameter.PageSize;
        var count = _source.Count();
        if (orderFunc == null)
        {
            items = _source
                .Skip(toSkip)
                .Take(paginationParameter.PageSize)
                .ToList();
        }
        else
        {
            var itemsOrderTempResult = paginationOrder switch
            {
                PaginationOrder.Ascending => _source.OrderBy(orderFunc),
                PaginationOrder.Descending => _source.OrderByDescending(orderFunc),
                _ => throw new ArgumentOutOfRangeException(nameof(paginationOrder), paginationOrder, null)
            };
            items = itemsOrderTempResult
                .Skip(toSkip)
                .Take(paginationParameter.PageSize);
        }

        var hasNext = count - paginationParameter.PageSize * paginationParameter.PageNumber > 0;
        var previousPageBuilder = new StringBuilder(paginationParameter.BaseUrl);
        var pageNumberName = paginationParameter.PageNumberName ?? nameof(paginationParameter.PageNumber);
        var urlSeparator = paginationParameter.BaseUrl.Contains(QuerySign) ? AmpersandSign : QuerySign;
        var pageSizeName = paginationParameter.PageSizeName ?? nameof(paginationParameter.PageSize);
        previousPageBuilder.Append(urlSeparator)
            .Append(pageNumberName)
            .Append(EqualsSign)
            .Append(paginationParameter.PageNumber - 1)
            .Append(AmpersandSign)
            .Append(pageSizeName)
            .Append(EqualsSign)
            .Append(paginationParameter.PageSize);
        var nextPageBuilder = new StringBuilder(paginationParameter.BaseUrl);
        nextPageBuilder.Append(urlSeparator)
            .Append(pageNumberName)
            .Append(EqualsSign)
            .Append(paginationParameter.PageNumber + 1)
            .Append(AmpersandSign)
            .Append(pageSizeName)
            .Append('=')
            .Append(paginationParameter.PageSize);
        return new UrlPage<T>(items, paginationParameter.PageNumber, paginationParameter.PageSize, hasNext,
            count, paginationParameter.BaseUrl, previousPageBuilder.ToString(),
            nextPageBuilder.ToString());
    }
}