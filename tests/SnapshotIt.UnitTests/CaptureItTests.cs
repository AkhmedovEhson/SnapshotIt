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
            var capture = new CaptureIt<o>(2);
            o op = new() { id = 1 };

            // Act
            capture.Post(op);
            op.id = 2;

            // Assert
            op.id.Should().Be(2);
            capture[0].id.Should().Be(1);
        }

        [Test]
        public void CaptureThrowsNullReference()
        {
            var capture = new CaptureIt<o>();

            Assert.Throws<NullReferenceException>(() => {
                var item = capture[0]; // throws, size not provided to `CaptureIt`
            });
        }

        [Test]
        public void CaptureThrowsOutOfRange()
        {
            var capture = new CaptureIt<o>(1);
            capture.Post(new o { id = 123 });

            Assert.Throws<IndexOutOfRangeException>(() => {
                var item = capture[1]; // out of size
            });
        }
    }
}
