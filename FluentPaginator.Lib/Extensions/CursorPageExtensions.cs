using System;
using System.Linq;
using FluentPaginator.Lib.Page;

namespace FluentPaginator.Lib.Extensions;

public static class CursorPageExtensions
{
    public static CursorPage<TU> Map<T, TU>(this CursorPage<T> self, Func<T, TU> mapper)
    {
        return new CursorPage<TU>(
            Items: self.Items.Select(mapper),
            PageSize: self.PageSize,
            Last: mapper(self.Last),
            Total: self.Total
        );
    }

    public static void ForEach<T>(this CursorPage<T> self, Action<T> action)
    {
        foreach (var item in self.Items)
        {
            action(item);
        }
    }
}