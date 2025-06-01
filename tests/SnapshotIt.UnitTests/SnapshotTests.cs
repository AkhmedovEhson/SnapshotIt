using SnapshotIt;
using FluentAssertions;
using SnapshotIt.Domain;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using SnapshotIt.Domain.Utils;
using SnapshotIt.UnitTests.TestObjects;

namespace SnapshotIt.UnitTests
{
    // todo: Fill some tests
    public class SnapshotItTests
    {
        [Test]
        public async Task Snapshot_Success()
        {
            var @object = new SimpleObject() { Id = 1,Name = "test"};
            const int _Itr = 1_000;

            for(int i = 0; i < _Itr; i++)
            {
                @object.Id = i;
                Snapshot.AsyncOut.PostToBuffers(@object);
            }

            IAsyncEnumerable<SimpleObject> enumerable =  Snapshot.AsyncOut.ReadFromBuffersLine<SimpleObject>();
            enumerable.Should().NotBeNull();
            enumerable.Should().NotBeOfType<IAsyncEnumerable<SimpleObject>>();
        }

        [Test]
        public void Snapshot_Failure()
        {
             // todo ...
        }



    }
}
