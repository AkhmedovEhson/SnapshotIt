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
            var data = new Snaps<o>(new o { id = 1 });
            var response = data.Get();
            response.Should().NotBeNull();
            response!.Value.Value.id.Should().Be(1);
          
        }

        [Test]
        public async Task Pick2()
        {
            const int iter = 100;
            var snapshot = new Snaps<o>();

            for (int i = 0; i <= iter; i++)
            {
                snapshot.Push(new o { id = i });
            }
            var check1 = await snapshot.GetAsync(55);
            check1.Should().NotBeNull();
            check1.Value?.id.Should().Be(55);

        }

        [Test]
        public async Task ThrowsOutOfRange()
        {
            var snapshot = new Snaps<o>();
            Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await snapshot.GetAsync(1));
        }



    }
}
