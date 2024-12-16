using FluentAssertions;
using SnapshotIt.Domain.Utils;
using SnapshotIt.UnitTests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.UnitTests
{

    public class CaptureItTests
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Snapshot.Out.Create<o>(100);
        }
        [Test]
        public void Capture_Post_Successfully()
        {
            // Arrange
            var obj = new o()
            {
                id = 1
            };

            //Act
            Snapshot.Out.Post<o>(obj);
            var result = Snapshot.Out.Get<o>(0);

            // Assert
            result.id.Should().Be(1);
        }

        [Test]
        public void CapturedObject_Throws_NullReferenceException_Expression()
        {

            Snapshot.Out.Post<o>(new o() { id = 1});

            Assert.Throws<NullReferenceException>(() => Snapshot.Out.Get<o>(i => i.id == 2));
        }

        [Test]
        public void CapturedObject_SuccessfullyFound_Expression()
        {
            Snapshot.Out.Post<o>(new o() { id = 1 });

            var result = Snapshot.Out.Get<o>(i => i.id == 1);
            result.Should().NotBeNull();
            result.id.Should().Be(1);
        }

        [Test]
        public void CapturedObject_Throws_NullReferenceException()
        {

            var result = Snapshot.Out.Get<o>(0);
            result.Should().BeNull();
        }

        [Test]
        public void CapturedObject_Throws_IndexOutOfRangeException()
        {
            Snapshot.Out.Create<o>(1);
            var collection = Snapshot.Out.GetAsEnumerable<o>();
            collection.Count().Should().Be(1);
            Snapshot.Out.Post(new o() { id = 1 });

            Assert.Throws<IndexOutOfRangeException>(() => {
                var item = Snapshot.Out.Get<o>(3); // out of size
            });
        }

        [Test]
        public void CollectionOfCaptures_GetAsSpan_Successfully()
        {
            // Arrange
            Snapshot.Out.Post(new o { id = 123 });

            // Act
            var span = Snapshot.Out.GetAsSpan<o>();


            // Assert
            bool result = span is Span<o>;
            result.Should().BeTrue();
        }

        [Test]
        public void CollectionOfCaptures_GetAsEnumerable_Successfully()
        {

            // Arrange
            Snapshot.Out.Post(new o { id = 123 });

            // Act
            var enumerable = Snapshot.Out.GetAsEnumerable<o>();

            // Assert
            Assert.IsInstanceOf<IEnumerable<o>>(enumerable);
        }

        [Test]
        public void CollectionOfCaptures_GetAsReadonlySpan_Successfully()
        {
            // Arrange
            Snapshot.Out.Post(new o { id = 123 });

            // Act
            var span = Snapshot.Out.GetAsReadonlySpan<o>();

            // Assert
            bool result = span is ReadOnlySpan<o>;
            result.Should().BeTrue();
        }
    }
}
