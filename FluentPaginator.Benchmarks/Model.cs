using System.ComponentModel.DataAnnotations;

namespace FluentPaginator.Benchmarks;

public class Model
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}