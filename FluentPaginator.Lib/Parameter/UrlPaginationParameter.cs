namespace FluentPaginator.Lib.Parameter;

/// <summary>
/// The parameter for the url pagination.
/// </summary>
/// <param name="PageSize">The size of one page of data</param>
/// <param name="PageNumber">The number of the current page</param>
/// <param name="BaseUrl">The base url of the app</param>
/// <param name="PageNumberName">The name of the url query param for the page number</param>
/// <param name="PageSizeName">The name of the url query param for the page size</param>
public record UrlPaginationParameter(int PageSize, int PageNumber, string BaseUrl, string? PageNumberName = null,
    string? PageSizeName = null) : PaginationParameter(PageSize,
    PageNumber);