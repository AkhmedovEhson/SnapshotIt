```

BenchmarkDotNet v0.13.8, Windows 11 (10.0.26100.4652)
11th Gen Intel Core i7-1165G7 2.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-SYTURP : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
| Method               | Mean       | Error     | StdDev     | Median     | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------- |-----------:|----------:|-----------:|-----------:|------:|--------:|----------:|------------:|
| Get_Sync             |   131.6 ns |  45.55 ns |   130.7 ns |   100.0 ns |     ? |       ? |     400 B |           ? |
| GetAsync_Comparison  | 3,834.4 ns | 351.15 ns | 1,013.2 ns | 3,500.0 ns |     ? |       ? |     544 B |           ? |
|                      |            |           |            |            |       |         |           |             |
| Post_Sync            |   546.9 ns | 107.09 ns |   309.0 ns |   450.0 ns |  1.00 |    0.00 |     400 B |        1.00 |
| PostAsync_Comparison |         NA |        NA |         NA |         NA |     ? |       ? |        NA |           ? |

Benchmarks with issues:
  CaptureItComparisonBenchmarks.PostAsync_Comparison: Job-SYTURP(InvocationCount=1, UnrollFactor=1)
