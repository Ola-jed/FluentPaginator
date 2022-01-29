using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    public UrlPage<T> Paginate<TKey>(UrlPaginationParameter paginationParameter, Func<T, TKey>? orderFunc = null)
    {
        IEnumerable<T> items;
        if (orderFunc == null)
        {
            items = _source
                .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .ToList();
        }
        else
        {
            items = _source.OrderBy(orderFunc)
                .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize);
        }

        var hasNext = _source.Count() - paginationParameter.PageSize * paginationParameter.PageNumber > 0;
        var previousPageBuilder = new StringBuilder(paginationParameter.BaseUrl);
        var pageNumberName = paginationParameter.PageNumberName ?? nameof(paginationParameter.PageNumber);
        var pageSizeName = paginationParameter.PageSizeName ?? nameof(paginationParameter.PageSize);
        previousPageBuilder.Append(paginationParameter.BaseUrl.Contains('?') ? '&' : '?')
            .Append(pageNumberName)
            .Append('=')
            .Append(paginationParameter.PageNumber - 1)
            .Append('&')
            .Append(pageSizeName)
            .Append('=')
            .Append(paginationParameter.PageSize);
        var nextPageBuilder = new StringBuilder(paginationParameter.BaseUrl);
        nextPageBuilder.Append(paginationParameter.BaseUrl.Contains('?') ? '&' : '?')
            .Append(pageNumberName)
            .Append('=')
            .Append(paginationParameter.PageNumber + 1)
            .Append('&')
            .Append(pageSizeName)
            .Append('=')
            .Append(paginationParameter.PageSize);
        return new UrlPage<T>(items, paginationParameter.PageNumber, paginationParameter.PageSize, hasNext,
            _source.Count(), paginationParameter.BaseUrl, previousPageBuilder.ToString(),
            nextPageBuilder.ToString());
    }
}