using System;
using System.Linq;
using FluentPaginator.Lib.Page;

namespace FluentPaginator.Lib.Extensions;

public static class PageExtensions
{
    public static Page<TU> Map<T, TU>(this Page<T> self, Func<T, TU> mapper)
    {
        return new Page<TU>(
            Items: self.Items.Select(mapper),
            Total: self.Total,
            PageNumber: self.PageNumber,
            PageSize: self.PageSize,
            HasNext: self.HasNext
        );
    }
    
    public static void ForEach<T>(this Page<T> self, Action<T> action)
    {
        foreach (var item in self.Items)
        {
            action(item);
        }
    }
}