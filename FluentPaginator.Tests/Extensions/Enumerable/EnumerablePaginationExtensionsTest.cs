using System.Linq;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using FluentPaginator.Tests.Setup;
using Xunit;

namespace FluentPaginator.Tests.Extensions.Enumerable;

public class EnumerablePaginationExtensionsTest
{
     [Fact]
    public void TestPaginatorOnIEnumerableWithRemainingPages()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 2), model => model.Id);
        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithNoRemainingPages()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 4), model => model.Id);
        Assert.Equal(5, pageResult.Items.Count());
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(4, pageResult.PageNumber);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithIncompletePage()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(6, 4), model => model.Id);
        Assert.Equal(2, pageResult.Items.Count());
        Assert.False(pageResult.HasNext);
        Assert.Equal(6, pageResult.PageSize);
        Assert.Equal(4, pageResult.PageNumber);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithEmptyPage()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 5), model => model.Id);
        Assert.Empty(pageResult.Items);
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(5, pageResult.PageNumber);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithoutOrdering()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate<Model, object>(new PaginationParameter(5, 2));
        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
    }
}