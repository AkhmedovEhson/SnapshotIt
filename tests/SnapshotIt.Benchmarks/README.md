# SnapshotIt Benchmarks

This project contains comprehensive performance benchmarks for the CaptureIt functionality in SnapshotIt.

## Benchmark Categories

### 1. Individual Method Benchmarks (`CaptureItIndividualBenchmarks`)

These benchmarks test the performance of individual CaptureIt methods:

- **Post_SingleProduct**: Tests synchronous posting of a single Product object
- **PostAsync_SingleProduct**: Tests asynchronous posting of a single Product object  
- **PostAsync_ProductArray**: Tests asynchronous posting of an array of 1000 Product objects
- **Get_ByIndex**: Tests synchronous retrieval by index
- **Get_ByExpression**: Tests synchronous retrieval using a lambda expression
- **GetAsync_ByIndex**: Tests asynchronous retrieval by index
- **GetAllAsync**: Tests asynchronous retrieval of all stored objects

### 2. Comparison Benchmarks (`CaptureItComparisonBenchmarks`)

These benchmarks provide head-to-head comparisons between sync and async methods:

- **Post_Sync vs PostAsync_Comparison**: Compares sync vs async posting performance
- **Get_Sync vs GetAsync_Comparison**: Compares sync vs async retrieval performance

The comparison benchmarks use baseline annotations to clearly show relative performance.

## How to Run Benchmarks

### Run All Benchmarks
```bash
cd tests/SnapshotIt.Benchmarks
dotnet run -c Release
```

### Run Specific Benchmark Class
```bash
# Individual method benchmarks
dotnet run -c Release -- --filter '*CaptureItIndividualBenchmarks*'

# Comparison benchmarks  
dotnet run -c Release -- --filter '*CaptureItComparisonBenchmarks*'
```

### Quick Dry Run
```bash
dotnet run -c Release -- -j Dry --filter '*CaptureItIndividualBenchmarks*'
```

### Export Results
```bash
# Export to multiple formats
dotnet run -c Release -- --exporters GitHub CSV JSON HTML
```

### Memory Analysis
```bash
# Include memory allocation analysis
dotnet run -c Release -- -m
```

## Benchmark Configuration

- **Memory Diagnostics**: Enabled to track memory allocations
- **Job Configuration**: Uses SimpleJob for consistent results
- **Test Data**: Uses Product objects with realistic properties (Id, Name, Price)
- **Collection Size**: Individual benchmarks use collections of 2000 items, comparison benchmarks use 200 items

## Interpreting Results

- **Mean**: Average execution time per operation
- **Error**: Half of 99.9% confidence interval
- **StdDev**: Standard deviation of all measurements
- **Median**: 50th percentile of all measurements
- **Allocated**: Total memory allocated per operation

For comparison benchmarks, look for:
- **Baseline**: The reference method (typically synchronous)
- **Ratio**: How much faster/slower the method is compared to baseline
- **Alloc Ratio**: Memory allocation comparison to baseline

## Performance Expectations

Based on the benchmark design:

1. **Post methods** should show minimal overhead for sync vs async with single objects
2. **PostAsync with arrays** should demonstrate the benefits of batch operations
3. **Get methods** may show async overhead for simple operations
4. **Expression-based Get** will be slower than index-based due to LINQ evaluation
5. **GetAllAsync** performance depends on collection size and concurrency patterns

## Notes

- Benchmarks run in Release configuration for accurate performance measurements
- Each benchmark includes proper setup/cleanup to ensure fair testing
- Test data is generated once and reused to minimize allocation noise
- Collections are reset between iterations to ensure consistent starting conditions