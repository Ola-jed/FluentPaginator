using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using Xunit;

namespace FluentPaginator.Tests.Core;

public class PageTest
{
    [Fact]
    public void TestMapOnPageReturnsCorrectData()
    {
        var page = new Page<int>(new[] { 1, 2, 3, 4, 5 }, 1, 5, false, 5);
        var mappedPage = page.Map(x => x * 2);
        
        Assert.Equal(5, mappedPage.Total);
        Assert.Equal(1, mappedPage.PageNumber);
        Assert.Equal(5, mappedPage.PageSize);
        Assert.Equal(5, mappedPage.Total);
        Assert.Equal(new[] { 2, 4, 6, 8, 10 }, mappedPage.Items);
    }
}