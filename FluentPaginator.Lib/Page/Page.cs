using System.Collections.Generic;

namespace FluentPaginator.Lib.Page;

/// <summary>
/// A page of paginated data
/// </summary>
/// <param name="Items">The collection of elements</param>
/// <param name="PageNumber">Number of the current page</param>
/// <param name="PageSize">Number of elements the page is supposed to contain</param>
/// <param name="HasNext">If a next page exists</param>
/// <param name="Total">The total number of items in the source data</param>
/// <typeparam name="T">The Type of the data contained</typeparam>
public record Page<T>(IEnumerable<T> Items, int PageNumber, int PageSize, bool HasNext, int Total);