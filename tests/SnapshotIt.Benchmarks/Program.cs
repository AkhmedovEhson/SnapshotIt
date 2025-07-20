
using BenchmarkDotNet.Attributes;
using SnapshotIt.Benchmarks.Types;
using BenchmarkDotNet.Running;
using SnapshotIt;
using BenchmarkDotNet.Configs;
using System.Text.Json;

class Program
{
    public static async Task Main(string[] args)
    {
        BenchmarkRunner.Run<CaptureItComparisonBenchmarks>();
    }
}

/// <summary>
/// Benchmarks for individual CaptureIt methods
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class CaptureItIndividualBenchmarks
{
    private Product[] _products = null!;
    private Product _singleProduct = null!;
    
    [GlobalSetup]
    public void Setup()
    {
        // Create test data
        _singleProduct = new Product { Id = 1, Name = "Benchmark Product", Price = 99.99m };
        
        _products = Enumerable.Range(1, 1000)
            .Select(i => new Product { Id = i, Name = $"Product {i}", Price = i * 10.5m })
            .ToArray();
            
        // Initialize CaptureIt collection with sufficient size
        Snapshot.Out.Create<Product>(2000);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        Snapshot.Out.Create<Product>(1);
    }

    [Benchmark]
    public void Post_SingleProduct()
    {
        Snapshot.Out.Post(_singleProduct);
    }
    
    [Benchmark] 
    public async Task PostAsync_SingleProduct()
    {
        await Snapshot.Out.PostAsync(_singleProduct);
    }
    
    [Benchmark]
    public async Task PostAsync_ProductArray()
    {
        await Snapshot.Out.PostAsync(_products);
    }
    
    [Benchmark]
    public Product Get_ByIndex()
    {
        return Snapshot.Out.Get<Product>(0);
    }
    
    [Benchmark]
    public Product Get_ByExpression()
    {
        return Snapshot.Out.Get<Product>(p => p.Id == 1);
    }
    
    [Benchmark]
    public async Task<Product> GetAsync_ByIndex()
    {
        return await Snapshot.Out.GetAsync<Product>(0);
    }
    
    [Benchmark]
    public async Task<Product[]> GetAllAsync()
    {
        return await Snapshot.Out.GetAllAsync<Product>();
    }
}

/// <summary>
/// Comparison benchmarks between sync and async methods
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class CaptureItComparisonBenchmarks
{
    private Product _testProduct = null!;
    private Product[] _testProducts = null!;
    
    [GlobalSetup] 
    public void Setup()
    {
        _testProduct = new Product { Id = 1, Name = "Test Product", Price = 49.99m };
        _testProducts = Enumerable.Range(1, 100)
            .Select(i => new Product { Id = i, Name = $"Product {i}", Price = i * 5.0m })
            .ToArray();
    }
    
    [IterationSetup]
    public void IterationSetup()
    {
        // Reset collection before each iteration
        Snapshot.Out.Create<Product>(200);
        
        // Pre-populate with test data for Get benchmarks
        for (int i = 0; i < 50; i++)
        {
            Snapshot.Out.Post(new Product { Id = i, Name = $"Setup Product {i}", Price = i * 2.0m });
        }
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Snapshot.Out.Create<Product>(1);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Post")]
    public void Post_Sync()
    {
        foreach(var item in _testProducts)
        {
            Snapshot.Out.Post(item);
        }
    }
    
    [Benchmark]
    [BenchmarkCategory("Post")]
    public async Task PostAsync_Comparison()
    {
        await Snapshot.Out.PostAsync(_testProducts);
    }
    
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Get")]
    public Product Get_Sync()
    {
        return Snapshot.Out.Get<Product>(10);
    }
    
    [Benchmark]
    [BenchmarkCategory("Get")]
    public async Task<Product> GetAsync_Comparison()
    {
        return await Snapshot.Out.GetAsync<Product>(10);
    }
}
