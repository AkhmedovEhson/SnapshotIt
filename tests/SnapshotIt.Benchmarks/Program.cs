
using BenchmarkDotNet.Attributes;
using SnapshotIt.Benchmark.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using BenchmarkDotNet.Running;
using SnapshotIt.Domain.Utils;

class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Testing>();
    }

}


[MemoryDiagnoser]
public class Testing
{

    // takes 4 or kb
    [Benchmark]
    public void CreateClasses()
    {
        Product[] arr = new Product[5];
        var product = new Product() { Id = 1, Name = "Nike" };
        for(int i = 0; i < 100; i++)
        {
            var product2 = new Product()
            {
                Id = i,
                Name = "Nike"
            };
            arr[i < arr.Length - 1 ? i : arr.Length - 1] = product2;
        }
    }

    // takes 13 kb;
    [Benchmark]
    public void PushPerformance()
    {
        var product = new Product() { Id = 1,Name = "Nike" };
        var snap = new Snaps<Product>();

        for(int i = 0; i < 100;i++)
        {
            product.Id = i;
            snap.Push(product);
        }
    }
}
