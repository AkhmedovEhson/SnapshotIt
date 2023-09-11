using SnapshotIt;
using SnapshotIt.Common.Services;
using FluentAssertions;
namespace SnapshotIt.UnitTests
{
    public class o
    {
        public int id { get; set; }   
    }
    public class Tests
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests() { }


        [Test]
        public void Pick()
        {

            var obj = new o { id = 123 };
            for(int i = 0; i < 100_000; i++)
            {
                Assert.DoesNotThrow(() => Snap<o>.Pick(obj));
            }
           
        }

        [Test]
        public void Clap()
        {
            var oj = new o() { id = 123 };
            Snap<o>.Pick(oj);

            var value = Snap<o>.ClapOne();

            value.Should().NotBeNull();

        }
    }
}