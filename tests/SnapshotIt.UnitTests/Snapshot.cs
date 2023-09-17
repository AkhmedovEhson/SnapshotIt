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
            var data = new Snap<o>(new o() { id = 2});
            data.Should().NotBeNull();
            data.ClapOne().Should().NotBeNull();
            data.ClapOne().Should().BeOfType<o>();

        }

        [Test]
        public void Pick2()
        {
            var data = new Snap<o>(new o { id = 3 });
            data.ClapOne()!.id.Should().Be(3);

            o _ = new o() { id = 1 };
            data.Pick(ref _);
            data.ClapOne()!.id.Should().Be(1);
        }

    }
}
