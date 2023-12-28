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
            Snapshot.Out.Create<int>(1);
            Snapshot.Out.Post(1234);
            var cache = Snapshot.Out.Get<int>(0);
            cache.Should().Be(1234);
          
        }



    }
}
