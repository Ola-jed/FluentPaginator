namespace FluentPaginator.Lib.Page;

/// <summary>
/// Item contained in a cursor for the cursor paginator.
/// </summary>
/// <param name="Id">Id of the element in the collection for ordering</param>
/// <param name="Item"></param>
/// <typeparam name="T"></typeparam>
public record CursorItem<T>(int Id, T Item);