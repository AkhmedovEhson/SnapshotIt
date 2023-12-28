using SnapshotIt;
using FluentAssertions;
using SnapshotIt.Domain;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using SnapshotIt.Domain.Utils;

namespace SnapshotIt.UnitTests
{
    public class o
    {
        public int id { get; set; }   
    }


    // todo: Fill some tests
    public class Tests
    {
        [Test]
        public async Task Pick()
        {
            Snapshot.Out.Post(1234);
            var cache = Snapshot.Out.Get<int>(0);
            cache.Should().NotBe(null);
            cache.Should().Be(1234);
          
        }



    }
}
