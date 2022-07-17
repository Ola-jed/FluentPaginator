# FluentPaginator

[![Coverage Status](https://coveralls.io/repos/github/Ola-jed/FluentPaginator/badge.svg?branch=dev)](https://coveralls.io/github/Ola-jed/FluentPaginator?branch=dev)

FluentPaginator is an easy to use library to help pagination with `IQueryable` and `IEnumerable`

## Download

[NuGet Gallery](https://www.nuget.org/packages/FluentPaginator.Lib/)

```shell
dotnet add package FluentPaginator.Lib
```

## Basic Usage (Reproducible example)

```c#
using System.ComponentModel.DataAnnotations;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.ConsoleSample;

public class Model
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
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
        Models.AddRange(Enumerable.Range(1, 20).Select(i => new Model { Id = i, Name = $"Item {i}" }));
        SaveChanges();
    }

    public DbSet<Model> Models { get; set; } = null!;
}

public static class Program
{
    public static void Main()
    {
        using var context = new Context(new DbContextOptionsBuilder<Context>().Options);
        var page = context.Models.Paginate(new PaginationParameter(5, 4), x => x.Id); // Ordering items with their Id
        Console.WriteLine($"Page {page.PageNumber}"); // Page 4
        Console.WriteLine($"Items per page : {page.PageSize}"); // Items per page : 5
        Console.WriteLine($"Has next : {page.HasNext}"); // Has next : False
        Console.WriteLine($"Total number of items : {page.Total}"); // Total number of items : 20
        foreach (var model in page.Items)
        {
            Console.WriteLine($"{model.Id} - {model.Name}"); // Will show the 5 last models from 16 to 20
        }

        // You can also paginate using the descending order
        var descendingOrderedPage = context.Models.Paginate(
            new PaginationParameter(5, 1),
            x => x.Id,
            PaginationOrder.Descending
        );
        foreach (var model in descendingOrderedPage.Items)
        {
            Console.WriteLine($"{model.Id} - {model.Name}"); // Will output the 5 last models from 20 to 16
        }
    }
}
```