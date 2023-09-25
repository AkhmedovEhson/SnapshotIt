using SnapshotIt;
using SnapshotIt.Common.Services;
using FluentAssertions;
using SnapshotIt.Domain;
using SnapshotIt.Common;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

namespace SnapshotIt.UnitTests
{
    public class o
    {
        public int id { get; set; }   
    }

    public class Tests
    {
        [Test]
        public void Pick()
        {
            var data = new Snap<o>(new o { id = 1 });
            var apple = data.ClapOne();

            apple.Should().NotBeNull();
            apple.Should().BeOfType<o>();
          
        }

        [Test]
        public void Pick2()
        {
            var snapshot = new Snap<o>();

            for(int i = 1; i <= 5; i++)
            {
                snapshot.Pick(new o { id = i });    
            }

            
            snapshot.snapshot_collection.Should().NotBeNull();
            snapshot.snapshot_collection.Should().HaveCount(5);
            snapshot.snapshot_collection.Should().NotContainNulls();

            snapshot.ClapOne(3).Value.Should().NotBeNull();
            snapshot.ClapOne(3).Value?.id.Should().Be(4);


        }

        [Test]
        public void ClapFail()
        {
            var snapshot = new Snap<o>();
            snapshot.ClapOne().Value.Should().BeNull();
            snapshot.ClapOne(2).Value.Should().BeNull();
        }

        [Test]
        public void ClapSuccess()
        {
            var snapshot = new Snap<o>(new o { id = 2});
            snapshot.ClapOne().Value.Should().NotBeNull();
            snapshot.ClapOne().Value!.id.Should().Be(2); 
            
           
        }

        [Test]
        public void ClapThrowsException()
        {
            var snapshot = new Snap<o>();
            Assert.Throws<IndexOutOfRangeException> (() => snapshot.ClapOne(10));
        }



    }
}
