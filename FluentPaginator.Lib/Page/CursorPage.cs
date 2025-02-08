using System;
using System.Collections.Generic;

namespace FluentPaginator.Lib.Page;

/// <summary>
/// A page of paginated data, created using cursors
/// </summary>
/// <param name="Items">The collection of elements</param>
/// <param name="Last">The last element of the collection, for a next request</param>
/// <param name="PageSize">Number of elements the page is supposed to contain</param>
/// <param name="Total">The total number of items in the source data</param>
/// <typeparam name="T">The Type of the data contained</typeparam>
public record CursorPage<T>(IEnumerable<T> Items, int PageSize, T? Last, int Total)
{
    public virtual bool Equals(CursorPage<T>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Items.Equals(other.Items)
               && (Last == null || Last.Equals(other.Last))
               && PageSize == other.PageSize
               && Total == other.Total;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Items, Last, PageSize, Total);
    }
}