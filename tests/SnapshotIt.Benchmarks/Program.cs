using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SnapshotIt.Benchmarks.Types;
using SnapshotIt.Common.Services;
using System.Text.Json;

BenchmarkRunner.Run<SnapshotPerformance>();


[MemoryDiagnoser]
public class SnapshotPerformance
{
    [Benchmark]
    public void PickPerformance()
    {
        var product = new Product()
        {
            Id = 1,
            Name = "Test",
            Price = 100
        };

        var snap = new Snap<Product>(product);
        for (int i = 0; i < 10; i++)
        {
            product.Id = i;
            snap.Pick(product);
        }
    }

    [Benchmark]
    public void PickClapPerformance()
    {
        var product = new Product()
        {
            Id = 1,
            Name = "Test",
            Price = 100
        };

        var snap = new Snap<Product>(product);
        for (int i = 0; i < 10; i++)
        {
            product.Id = i;
            snap.Pick(product);
        }

        for (int i = 0; i < 5; i++)
        {
            snap.ClapOne(i);
        }
        snap.Clear();
    }


}

