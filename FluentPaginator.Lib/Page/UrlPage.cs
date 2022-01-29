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
/// <param name="BaseUrl">The url for querying this data</param>
/// <param name="PreviousUrl">The url of the previous page</param>
/// <param name="NextUrl">The url of the next page</param>
/// <typeparam name="T">The Type of the data contained</typeparam>
public record UrlPage<T>(IEnumerable<T> Items, int PageNumber, int PageSize, bool HasNext, int Total, string BaseUrl,
    string PreviousUrl, string NextUrl) : Page<T>(Items, PageNumber, PageSize, HasNext, Total);