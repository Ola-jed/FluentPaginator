using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentPaginator.Lib.Core.Interfaces;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.Lib.Core;

/// <summary>
///     The Url Paginator for web apps
/// </summary>
/// <typeparam name="T">The type that will be in the pagination data</typeparam>
public class UrlPaginator<T> : IUrlPaginator<T>
{
    private const char AmpersandSign = '&';
    private const char QuerySign = '?';
    private const char EqualsSign = '=';

    private readonly IQueryable<T> _source;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="source">The IQueryable used as source data</param>
    public UrlPaginator(IQueryable<T> source)
    {
        _source = source;
    }

    public UrlPage<T> Paginate<TKey>(UrlPaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CountStrategy countStrategy = CountStrategy.Normal)
    {
        IEnumerable<T> items;
        var toSkip = (paginationParameter.PageNumber - 1) * paginationParameter.PageSize;
        var count = countStrategy switch
        {
            CountStrategy.Normal => _source.Count(),
            CountStrategy.NoCount => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(countStrategy), countStrategy, null)
        };
        
        if (orderFunc == null)
        {
            items = _source
                .Skip(toSkip)
                .Take(paginationParameter.PageSize + 1)
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
                .Take(paginationParameter.PageSize + 1);
        }

        var hasNext = items.Count() > paginationParameter.PageSize;
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
        return new UrlPage<T>(items.Take(paginationParameter.PageSize), paginationParameter.PageNumber,
            paginationParameter.PageSize, hasNext, count, paginationParameter.BaseUrl, 
            previousPageBuilder.ToString(), nextPageBuilder.ToString());
    }

    public async Task<UrlPage<T>> AsyncPaginate<TKey>(UrlPaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CountStrategy countStrategy = CountStrategy.Normal,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<T> items;
        var toSkip = (paginationParameter.PageNumber - 1) * paginationParameter.PageSize;
        var count = countStrategy switch
        {
            CountStrategy.Normal => await _source.CountAsync(cancellationToken),
            CountStrategy.NoCount => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(countStrategy), countStrategy, null)
        };
        
        if (orderFunc == null)
        {
            items = await _source
                .Skip(toSkip)
                .Take(paginationParameter.PageSize + 1)
                .ToListAsync(cancellationToken);
        }
        else
        {
            var itemsOrderTempResult = paginationOrder switch
            {
                PaginationOrder.Ascending => _source.OrderBy(orderFunc),
                PaginationOrder.Descending => _source.OrderByDescending(orderFunc),
                _ => throw new ArgumentOutOfRangeException(nameof(paginationOrder), paginationOrder, null)
            };
            items = await itemsOrderTempResult
                .Skip(toSkip)
                .Take(paginationParameter.PageSize + 1)
                .ToListAsync(cancellationToken);
        }

        var hasNext = items.Count() > paginationParameter.PageSize;
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
        return new UrlPage<T>(items.Take(paginationParameter.PageSize), paginationParameter.PageNumber,
            paginationParameter.PageSize, hasNext, count, paginationParameter.BaseUrl,
            previousPageBuilder.ToString(), nextPageBuilder.ToString());
    }
}