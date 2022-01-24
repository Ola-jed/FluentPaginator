using System.ComponentModel.DataAnnotations;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.ConsoleSample;

public class Model
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Seed();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("db");
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
    public DbSet<Model> Models { get; set; } = null!;
}

public static class Program
{
    public static void Main()
    {
        using var context = new Context(new DbContextOptionsBuilder<Context>().EnableSensitiveDataLogging().Options);
        var data = context.Models.Paginate(new PaginationParameter(5, 4), x => x.Id);
        Console.WriteLine($"Page {data.PageNumber}");
        Console.WriteLine($"Items per page : {data.PageSize}");
        Console.WriteLine($"Has next : {data.HasNext}");
        foreach (var model in data.Items)
        {
            Console.WriteLine($"{model.Id} - {model.Name}");
        }
    }
}