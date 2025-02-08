using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using Xunit;

namespace FluentPaginator.Tests.Core;

public class CursorPageTest
{
    [Fact]
    public void TestMapOnPageReturnsCorrectData()
    {
        var page = new CursorPage<int>([1, 2, 3, 4, 5], 5, 5, 5);
        var mappedPage = page.Map(x => x * 2);

        Assert.Equal(5, mappedPage.Total);
        Assert.Equal(5, mappedPage.PageSize);
        Assert.Equal(5, mappedPage.Total);
        Assert.Equal([2, 4, 6, 8, 10], mappedPage.Items);
    }
}