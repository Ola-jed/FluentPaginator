using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.Benchmarks;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Seed();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }

    private void Seed()
    {
        var items = new List<Model>();
        for (var i = 0; i < 20; i++)
        {
            items.Add(new Model { Name = $"Item {i}" });
        }

        Models.AddRange(items);
        SaveChanges();
    }

    public static Context Build()
    {
        return new Context(new DbContextOptions<Context>());
    }

    public DbSet<Model> Models { get; set; } = null!;
}