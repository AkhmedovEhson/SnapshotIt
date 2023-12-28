using FluentAssertions;
using SnapshotIt.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.UnitTests
{
    public class CaptureItTests
    {
        [Test]
        public void CaptureSuccessfully()
        {
            // Arrange
            var obj = new o()
            {
                id = 1
            };
            Snapshot.Out.Create<o>(2);

            //Act
            Snapshot.Out.Post<o>(obj);
            var result = Snapshot.Out.Get<o>(0);

            // Assert
            result.id.Should().Be(1);
        }

        [Test]
        public void CaptureThrowsNullReference()
        {
            Snapshot.Out.Create<o>(1);

            var result = Snapshot.Out.Get<o>(0);
            result.Should().BeNull();
        }

        [Test]
        public void CaptureThrowsOutOfRange()
        {
            Snapshot.Out.Create<o>(1);
            Snapshot.Out.Post(new o() { id = 1 });

            Assert.Throws<IndexOutOfRangeException>(() => {
                var item = Snapshot.Out.Get<o>(3); // out of size
            });
        }

        [Test]
        public void GetAsSpan()
        {
            // Arrange
            Snapshot.Out.Create<o>(1);
            Snapshot.Out.Post(new o { id = 123 });

            // Act
            var span = Snapshot.Out.GetAsSpan<o>();


            // Assert
            span.Length.Should().Be(1);        
        }

        [Test]
        public void GetAsEnumerable()
        {

            // Arrange
            Snapshot.Out.Create<o>(1);
            Snapshot.Out.Post(new o { id = 123 });

            // Act
            var enumerable = Snapshot.Out.GetAsEnumerable<o>();
            enumerable.Count().Should().Be(1);

            // Assert
            Assert.IsInstanceOf<IEnumerable<o>>(enumerable);
        }

        [Test]
        public void GetAsReadonlySpan()
        {
            // Arrange
            Snapshot.Out.Create<o>(1);
            Snapshot.Out.Post(new o { id = 123 });

            // Act
            var span = Snapshot.Out.GetAsReadonlySpan<o>();
            
            // Assert
            span.Length.Should().Be(1);
        }
    }
}
