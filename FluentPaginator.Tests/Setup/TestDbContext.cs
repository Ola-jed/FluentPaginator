using System;
using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.Tests.Setup;

public class TestDbContext: DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
        Seed();
    }
    
    public DbSet<Model> Models { get; set; } = null!;

    private void Seed()
    {
        var items = new Model[20];
        for (var i = 0; i < 20; i++)
        {
            items[i] = new Model { Name = $"Item {i}" };
        }
        Models.AddRange(items);
        SaveChanges();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
}