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

    public class Tests
    {
        [Test]
        public void Pick()
        {
            var data = new Snaps<o>(new o { id = 1 });
            var apple = data.Get();

            apple.Should().NotBeNull();
            apple.Value.Value.id.Should().Be(1);
          
        }

        [Test]
        public void Pick2()
        {
            var snapshot = new Snaps<o>();

            for (int i = 0; i <= 100; i++)
            {
                snapshot.Push(new o { id = i });
            }

            snapshot.Get(4).Value.Should().NotBeNull();
            snapshot.Get(4).Value?.id.Should().Be(100);

            Assert.Throws<IndexOutOfRangeException>(() => snapshot.Get(12));
        }

        [Test]
        public void ClapFail()
        {
            var snapshot = new Snaps<o>();
            snapshot.Get().Value.Value.Should().BeNull();
            snapshot.Get(2).Value.Should().BeNull();
        }

        [Test]
        public void ClapSuccess()
        {
            var snapshot = new Snaps<o>();
            snapshot.Push(new o { id = 2 });
            snapshot.Get()?.Value.Should().NotBeNull();
            snapshot.Get()?.Value.id.Should().Be(2); 
            
           
        }

        [Test]
        public void ClapThrowsException()
        {
            var snapshot = new Snaps<o>();
            Assert.Throws<IndexOutOfRangeException> (() => snapshot.Get(10));
        }



    }
}
