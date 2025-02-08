namespace FluentPaginator.Lib.Parameter;

/// <summary>
/// The parameter for the pagination.
/// </summary>
/// <param name="PageSize">The size of one page of data</param>
/// <param name="PageNumber">The number of the current page</param>
public record PaginationParameter(int PageSize, int PageNumber);