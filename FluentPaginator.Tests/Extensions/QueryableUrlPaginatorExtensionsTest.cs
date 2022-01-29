using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using Xunit;
using FluentPaginator.Tests.Setup;

namespace FluentPaginator.Tests.Extensions;

public class QueryableUrlPaginatorExtensionsTest
{
    [Fact]
    public void TestUrlPaginateWithoutQueryParameter()
    {
        using var context = TestContextBuilder.Build();
        var paginationParam = new UrlPaginationParameter(5,1, "http://localhost/test");
        var pageResult = context.Models.AsQueryable().UrlPaginate(paginationParam, x => x.Id);
        Assert.Equal(1, pageResult.PageNumber);
        Assert.Equal(5, pageResult.PageSize);
        Assert.True(pageResult.HasNext);
        Assert.Equal("http://localhost/test", pageResult.BaseUrl);
        Assert.Equal("http://localhost/test?PageNumber=0&PageSize=5", pageResult.PreviousUrl);
        Assert.Equal("http://localhost/test?PageNumber=2&PageSize=5", pageResult.NextUrl);
    }
    
    [Fact]
    public void TestUrlPaginateWithQueryParameter()
    {
        using var context = TestContextBuilder.Build();
        var paginationParam = new UrlPaginationParameter(5,1, "http://localhost/test?search=test");
        var pageResult = context.Models.AsQueryable().UrlPaginate(paginationParam, x => x.Id);
        Assert.Equal(1, pageResult.PageNumber);
        Assert.Equal(5, pageResult.PageSize);
        Assert.True(pageResult.HasNext);
        Assert.Equal("http://localhost/test?search=test", pageResult.BaseUrl);
        Assert.Equal("http://localhost/test?search=test&PageNumber=0&PageSize=5", pageResult.PreviousUrl);
        Assert.Equal("http://localhost/test?search=test&PageNumber=2&PageSize=5", pageResult.NextUrl);
    }
}