using System.Linq;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using FluentPaginator.Tests.Setup;
using Xunit;

namespace FluentPaginator.Tests.Extensions.Enumerable;

public class EnumerablePaginationExtensionsTest
{
    [Fact]
    public void TestPaginatorOnIEnumerableWithRemainingPages_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 2), model => model.Id,
            countStrategy: CountStrategy.Normal);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithRemainingPages_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 2), model => model.Id,
            countStrategy: CountStrategy.NoCount);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total); 
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithNoRemainingPages_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 4), model => model.Id,
            countStrategy: CountStrategy.Normal);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(4, pageResult.PageNumber);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithNoRemainingPages_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 4), model => model.Id,
            countStrategy: CountStrategy.NoCount);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(4, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithIncompletePage_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(6, 4), model => model.Id,
            countStrategy: CountStrategy.Normal);

        Assert.Equal(2, pageResult.Items.Count());
        Assert.False(pageResult.HasNext);
        Assert.Equal(6, pageResult.PageSize);
        Assert.Equal(4, pageResult.PageNumber);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithIncompletePage_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(6, 4), model => model.Id,
            countStrategy: CountStrategy.NoCount);

        Assert.Equal(2, pageResult.Items.Count());
        Assert.False(pageResult.HasNext);
        Assert.Equal(6, pageResult.PageSize);
        Assert.Equal(4, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithEmptyPage_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 5), model => model.Id,
            countStrategy: CountStrategy.Normal);

        Assert.Empty(pageResult.Items);
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(5, pageResult.PageNumber);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithEmptyPage_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 5), model => model.Id,
            countStrategy: CountStrategy.NoCount);

        Assert.Empty(pageResult.Items);
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(5, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithoutOrdering_NormalStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .Paginate<Model, object>(new PaginationParameter(5, 2), countStrategy: CountStrategy.Normal);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerableWithoutOrdering_NoCountStrategy()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable()
            .Paginate<Model, object>(new PaginationParameter(5, 2), countStrategy: CountStrategy.NoCount);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerable_DefaultStrategyIsNormal()
    {
        using var context = TestContextBuilder.Build();
        var pageResult = context.Models.AsEnumerable().Paginate(new PaginationParameter(5, 2), model => model.Id);

        Assert.Equal(5, pageResult.Items.Count());
        Assert.True(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(2, pageResult.PageNumber);
        Assert.Equal(20, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerable_EmptyCollection_NormalStrategy()
    {
        var emptyModels = System.Linq.Enumerable.Empty<Model>().AsEnumerable();
        var pageResult = emptyModels.Paginate(new PaginationParameter(5, 1), model => model.Id,
            countStrategy: CountStrategy.Normal);

        Assert.Empty(pageResult.Items);
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(1, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total);
    }

    [Fact]
    public void TestPaginatorOnIEnumerable_EmptyCollection_NoCountStrategy()
    {
        var emptyModels = System.Linq.Enumerable.Empty<Model>().AsEnumerable();
        var pageResult = emptyModels.Paginate(new PaginationParameter(5, 1), model => model.Id,
            countStrategy: CountStrategy.NoCount);
        Assert.Empty(pageResult.Items);
        Assert.False(pageResult.HasNext);
        Assert.Equal(5, pageResult.PageSize);
        Assert.Equal(1, pageResult.PageNumber);
        Assert.Equal(0, pageResult.Total);
    }
}