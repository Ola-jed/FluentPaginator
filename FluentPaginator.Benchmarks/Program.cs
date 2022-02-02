using BenchmarkDotNet.Running;
using FluentPaginator.Benchmarks;

var summary = BenchmarkRunner.Run<BenchmarkFluentPaginator>();
Console.WriteLine("Benchmarks ended");