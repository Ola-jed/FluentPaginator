namespace FluentPaginator.Lib.Parameter;

/// <summary>
/// The parameter for the cursor pagination.
/// </summary>
/// <param name="PageSize">The size of the page</param>
public record CursorPaginationParameter(int PageSize);