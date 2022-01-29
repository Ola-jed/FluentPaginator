using System;
using System.Linq;
using FluentPaginator.Lib.Core.Interfaces;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Core;

/// <summary>
/// Cursor pagination class
/// </summary>
/// <typeparam name="T">The data type to paginate</typeparam>
public class CursorPaginator<T> : IPaginator<T>
{
    private readonly IQueryable<CursorItem<T>> _cursorItems;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="source">The IQueryable use for pagination as source</param>
    public CursorPaginator(IQueryable<T> source)
    {
        var id = 0;
        _cursorItems = source.Select(element => new CursorItem<T>(IncrementId(ref id), element));
    }

    /// <summary>
    /// Paginate a source of data
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size and the current page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    /// <exception cref="ArgumentException">If the orderFunc given is not null</exception>
    public Page<T> Paginate<TKey>(PaginationParameter paginationParameter,
        Func<T, TKey>? orderFunc = null)
    {
        if (orderFunc != null)
        {
            throw new ArgumentException($"{nameof(orderFunc)} must be null");
        }

        var (pageSize, pageNumber) = paginationParameter;
        var items = _cursorItems
            .Where(item => item.Id > pageSize * pageNumber)
            .Take(pageSize)
            .Select(cursorItem => cursorItem.Item)
            .ToList();
        var hasNext = _cursorItems.Count() - pageSize * pageNumber > 0;
        return new Page<T>(items, pageNumber, pageSize, hasNext,_cursorItems.Count());
    }

    private static int IncrementId(ref int id)
    {
        return ++id;
    }
}