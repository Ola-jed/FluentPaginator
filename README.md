# FluentPaginator

[![Coverage Status](https://coveralls.io/repos/github/Ola-jed/FluentPaginator/badge.svg?branch=dev)](https://coveralls.io/github/Ola-jed/FluentPaginator?branch=dev)

FluentPaginator is a lightweight and easy-to-use library for paginating collections (`IQueryable` and `IEnumerable`) in .NET. It supports **page-based**, **cursor-based**, and **URL-based** pagination, with optional async support and configurable count strategies.

---

## Table of Contents

* [Installation](#installation)
* [Basic Page-Based Pagination](#basic-page-based-pagination)
* [Cursor-Based Pagination](#cursor-based-pagination)
* [URL-Based Pagination](#url-based-pagination)
* [Async Pagination](#async-pagination)
* [Count Strategies](#count-strategies)
* [License](#license)

---

## Installation

Install via NuGet:

```bash
dotnet add package FluentPaginator.Lib
```

Or add the following to your .csproj file

```xml
<PackageReference Include="FluentPaginator.Lib" Version="6.2.0" />
```

---

## Basic Pagination

Page-based pagination is the standard approach using **page number** and **page size**. It works with both `IQueryable` (e.g., EF Core) and `IEnumerable`.

```csharp
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;

// IQueryable example (Assuming you have a db context with a 'Models' dbset)
var page = context.Models.Paginate(new PaginationParameter(5, 2), x => x.Id);

// IEnumerable example
var enumerablePage = myList.Paginate(new PaginationParameter(5, 2), x => x.Id);

// Page properties
Console.WriteLine(page.PageNumber); // Current page
Console.WriteLine(page.PageSize); // Items per page
Console.WriteLine(page.HasNext); // Is there a next page
Console.WriteLine(page.Total); // Total number of items
```

---

## Cursor-Based Pagination

Cursor pagination is useful for large datasets,
because they do not require `offset` and `limit`
operations which can be costly.
Pages are defined relative to a **cursor**, usually the
last item of the previous page.

```csharp
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;

// IEnumerable example
var cursorPage = myList.CursorPaginate(new CursorPaginationParameter(5), firstKey: null, x => x.Id);

// Map or iterate items
cursorPage.ForEach(item => Console.WriteLine(item.Id));
var mapped = cursorPage.Map(x => new { x.Id, x.Name });
```

* `CursorPage<T>` provides the items, page size, total count, and the last element (`Last`) to continue pagination.

---

## URL-Based Pagination

URL pagination generates URLs for previous and next pages, useful for HTTP APIs.

```csharp
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Parameter;

// IEnumerable example
var urlPage = myList.UrlPaginate(new UrlPaginationParameter(5, 2), x => x.Id);

Console.WriteLine(urlPage.PreviousUrl); // URL for previous page
Console.WriteLine(urlPage.NextUrl); // URL for next page
```

* `UrlPage<T>` inherits from `Page<T>` and adds `BaseUrl`, `PreviousUrl`, and `NextUrl`.

---

## Async Pagination

When working with `IQueryable` (EF Core), you can paginate asynchronously:

```csharp
using FluentPaginator.Lib.Core.Interfaces;
using FluentPaginator.Lib.Parameter;

var paginator = new Paginator<Model>(context.Models);
var asyncPage = await paginator.AsyncPaginate(new PaginationParameter(5, 1), x => x.Id);
```

* Async pagination respects the same page size, ordering, and count strategies.

---

## Count Strategies

You can control how the total number of items is calculated:

* `CountStrategy.Normal` : computes the exact total.
* `CountStrategy.NoCount` : skips counting, returning `0` for
  total (faster, to prefer if the total size is not needed).

```csharp
var page = query.Paginate(new PaginationParameter(5, 1), countStrategy: CountStrategy.NoCount);
```
---

## License

FluentPaginator is licensed under the [MIT License](LICENSE).

---