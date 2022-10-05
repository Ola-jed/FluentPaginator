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
        Models.AddRange(Enumerable.Range(1, 500).Select(i => new Model { Id = i, Name = $"Item {i}" }));
        SaveChanges();
    }

    public static Context Build()
    {
        return new Context(new DbContextOptions<Context>());
    }

    public DbSet<Model> Models { get; set; } = null!;
}