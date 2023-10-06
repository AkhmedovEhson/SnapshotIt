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
            apple.Value.id.Should().Be(1);
          
        }

        [Test]
        public void Pick2()
        {
            var snapshot = new Snap<o>();

            for (int i = 0; i <= 100; i++)
            {
                snapshot.Pick(new o { id = i });
            }

            snapshot.ClapOne(4).Value.Should().NotBeNull();
            snapshot.ClapOne(4).Value?.id.Should().Be(100);

            Assert.Throws<IndexOutOfRangeException>(() => snapshot.ClapOne(12));
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
