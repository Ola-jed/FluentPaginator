using System.Linq;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using FluentPaginator.Tests.Setup;
using Xunit;

namespace FluentPaginator.Tests.Extensions.Enumerable;

public class EnumerableCursorPaginationExtensionsTest
{
    [Fact]
    public void TestPaginatorOnIEnumerableWithRemainingPages_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), null, model => model.Id,
                countStrategy: CountStrategy.Normal);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(5, pageResult.Last!.Id);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithRemainingPages_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), null, model => model.Id,
                countStrategy: CountStrategy.NoCount);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(5, pageResult.Last!.Id);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithIncompletePage_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), 18, model => model.Id,
                countStrategy: CountStrategy.Normal);

        Assert.Equal(2, pageResult.Items.Count());
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithIncompletePage_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), 18, model => model.Id,
                countStrategy: CountStrategy.NoCount);

        Assert.Equal(2, pageResult.Items.Count());
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithEmptyPage_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), 28, model => model.Id,
                countStrategy: CountStrategy.Normal);

        Assert.Empty(pageResult.Items);
        Assert.Null(pageResult.Last);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithEmptyPage_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), 28, model => model.Id,
                countStrategy: CountStrategy.NoCount);

        Assert.Empty(pageResult.Items);
        Assert.Null(pageResult.Last);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerable_DefaultStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .CursorPaginate(new CursorPaginationParameter(5), null, model => model.Id);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.Equal(context.Models.Count(), pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerable_EmptyCollection_NormalStrategy()
    {
        var emptyModels = System.Linq.Enumerable.Empty<Model>().AsEnumerable();
        var pageResult = emptyModels
            .CursorPaginate(new CursorPaginationParameter(5), null, model => model.Id,
                countStrategy: CountStrategy.Normal);

        Assert.Empty(pageResult.Items);
        Assert.Null(pageResult.Last);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerable_EmptyCollection_NoCountStrategy()
    {
        var emptyModels = System.Linq.Enumerable.Empty<Model>().AsEnumerable();
        var pageResult = emptyModels
            .CursorPaginate(new CursorPaginationParameter(5), null, model => model.Id,
                countStrategy: CountStrategy.NoCount);
        Assert.Empty(pageResult.Items);
        Assert.Null(pageResult.Last);
        Assert.Equal(0, pageResult.Total);
    }
}