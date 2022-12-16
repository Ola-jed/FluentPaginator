using System;
using System.Linq;
using FluentPaginator.Lib.Page;

namespace FluentPaginator.Lib.Extensions;

public static class UrlPageExtensions
{
    public static UrlPage<TU> Map<T, TU>(this UrlPage<T> self, Func<T, TU> mapper)
    {
        return new UrlPage<TU>(
            self.Items.Select(mapper),
            Total: self.Total,
            PageNumber: self.PageNumber,
            PageSize: self.PageSize,
            HasNext: self.HasNext,
            BaseUrl: self.BaseUrl,
            PreviousUrl: self.PreviousUrl,
            NextUrl: self.NextUrl
        );
    }

    public static void ForEach<T>(this UrlPage<T> self, Action<T> action)
    {
        foreach (var item in self.Items)
        {
            action(item);
        }
    }
}