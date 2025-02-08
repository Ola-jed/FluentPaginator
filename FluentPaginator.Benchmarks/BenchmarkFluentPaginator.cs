using BenchmarkDotNet.Attributes;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Benchmarks;

[MarkdownExporter, HtmlExporter]
public class BenchmarkFluentPaginator
{
    private readonly Context _context;

    public BenchmarkFluentPaginator()
    {
        _context = Context.Build();
    }

    [Benchmark]
    public Page<Model> Paginate()
    {
        return _context.Models.Paginate(new PaginationParameter(1, 10), m => m.Id);
    }

    [Benchmark]
    public CursorPage<Model> CursorPaginate()
    {
        return _context.Models.CursorPaginate(new CursorPaginationParameter(1), null, m => m.Id);
    }

    [Benchmark]
    public UrlPage<Model> UrlPaginate()
    {
        return _context.Models.UrlPaginate(new UrlPaginationParameter(1, 10, "http://localhost/models"), m => m.Id);
    }

    [Benchmark]
    public UrlPage<Model> UrlPaginateWithQueryParam()
    {
        return _context.Models.UrlPaginate(new UrlPaginationParameter(1, 10, "http://localhost/models?search=hallo"),
            m => m.Id);
    }

    [Benchmark]
    public UrlPage<Model> UrlPaginateWithPageNames()
    {
        return _context.Models.UrlPaginate(
            new UrlPaginationParameter(1, 10, "http://localhost/models", "Page", "PerPage"),
            m => m.Id);
    }

    [Benchmark]
    public UrlPage<Model> UrlPaginateWithPageNamesAndQueryParams()
    {
        return _context.Models.UrlPaginate(
            new UrlPaginationParameter(1, 10, "http://localhost/models?search=hallo", "Page", "PerPage"),
            m => m.Id);
    }
}