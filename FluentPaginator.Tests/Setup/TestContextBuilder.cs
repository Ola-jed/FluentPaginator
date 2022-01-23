using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.Tests.Setup;

public static class TestContextBuilder
{
    public static TestDbContext Build()
    {
        return new TestDbContext(new DbContextOptions<TestDbContext>());
    }
}