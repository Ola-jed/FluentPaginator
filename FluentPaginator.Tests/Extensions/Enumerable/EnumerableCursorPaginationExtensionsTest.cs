using System.Linq;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using FluentPaginator.Tests.Setup;
using Xunit;

namespace FluentPaginator.Tests.Extensions.Enumerable;

public class EnumerableCursorPaginationExtensionsTest
{
    [Fact]
    public void TestPaginatorOnIEnumerableWithRemainingPages()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), null, model => model.Id);
        Assert.Equal(5, pageResult.Items.Count());
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(5, pageResult.Last!.Id);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithIncompletePage()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), 18, model => model.Id);
        Assert.Equal(2, pageResult.Items.Count());
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithEmptyPage()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), 28, model => model.Id);
        Assert.Empty(pageResult.Items);
        Assert.Null(pageResult.Last);
    }
}