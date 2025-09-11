namespace FluentPaginator.Lib.Core;

/// <summary>
/// Strategy specifying how will the total be determined during the pagination.
/// </summary>
public enum CountStrategy
{
    /// <summary>
    /// Get the exact total of elements in the item set.
    /// </summary>
    Normal,
    
    /// <summary>
    /// The count is not computed, the value 0 is always returned.
    /// </summary>
    NoCount
}