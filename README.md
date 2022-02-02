# FluentPaginator

[![Coverage Status](https://coveralls.io/repos/github/Ola-jed/FluentPaginator/badge.svg?branch=dev)](https://coveralls.io/github/Ola-jed/FluentPaginator?branch=dev)

FluentPaginator is an easy to use library to help pagination with `IQueryable` and `IEnumerable`

## Download

[NuGet Gallery](https://www.nuget.org/packages/FluentPaginator.Lib/)

```shell
dotnet add package FluentPaginator.Lib
```

## Basic Usage

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
        using var context = new Context(new DbContextOptionsBuilder<Context>().Options);
        var data = context.Models.Paginate(new PaginationParameter(5, 4), x => x.Id);
        Console.WriteLine($"Page {data.PageNumber}"); // Page 4
        Console.WriteLine($"Items per page : {data.PageSize}"); // Items per page : 5
        Console.WriteLine($"Has next : {data.HasNext}"); // Has next : False
        Console.WriteLine($"Total number of items : {data.Total}"); // Total number of items : 20
        foreach (var model in data.Items)
        {
            Console.WriteLine($"{model.Id} - {model.Name}");
        }// 16 - Item 15
         // 17 - Item 16
         // 18 - Item 17
         // 19 - Item 18
         // 20 - Item 19
    }
}
```