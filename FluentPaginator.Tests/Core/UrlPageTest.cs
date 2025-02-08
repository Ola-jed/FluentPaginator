using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using Xunit;

namespace FluentPaginator.Tests.Core;

public class UrlPageTest
{
    [Fact]
    public void TestMapOnUrlPageReturnsCorrectData()
    {
        var page = new UrlPage<int>([1, 2, 3, 4, 5], 1, 5, false, 5, "http://localhost:8000/api/Items",
            "http://localhost:8000/api/Items?page=1", "http://localhost:8000/api/Items?page=3");
        
        var mappedPage = page.Map(x => x * 2);

        Assert.Equal(5, mappedPage.Total);
        Assert.Equal(1, mappedPage.PageNumber);
        Assert.Equal(5, mappedPage.PageSize);
        Assert.Equal(5, mappedPage.Total);
        Assert.Equal([2, 4, 6, 8, 10], mappedPage.Items);
        Assert.Equal("http://localhost:8000/api/Items", mappedPage.BaseUrl);
        Assert.Equal("http://localhost:8000/api/Items?page=1", mappedPage.PreviousUrl);
        Assert.Equal("http://localhost:8000/api/Items?page=3", mappedPage.NextUrl);
    }
}