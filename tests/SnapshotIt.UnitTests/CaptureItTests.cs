using FluentAssertions;
using SnapshotIt.Domain.Utils;
using SnapshotIt.UnitTests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapshotIt;
using System.Xml.Serialization;

namespace SnapshotIt.UnitTests
{

    public class CaptureItTests
    {
        private const int _defaultSizeOfSnapshots = 100;
        [SetUp]
        public void RunBeforeAnyTests()
        { 
           
            Snapshot.Out.Create<o>(_defaultSizeOfSnapshots);
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
        public async Task CapturedObject_GetAsync_Success()
        {
            var obj = new o[] 
            {
                new o()
                {
                    id = 1
                } 
            };
  
            await Snapshot.Out.PostAsync<o>(obj);
            var result = await Snapshot.Out.GetAsync<o>(0);

            result.Should().NotBeNull();
            result.id.Should().Be(1);
        }

        [Test]
        public async Task CapturedObject_PostAsync_GetAllAsync_ArrayTimesToTwo_Success()
        {
            var obj = new o[]
            {
                new o() { id = 1 },
                new o() { id = 2 },
                new o() { id = 3 }
            };

            Snapshot.Out.Create<o>(2);
            await Snapshot.Out.PostAsync<o>(obj);
            var result = await Snapshot.Out.GetAllAsync<o>();

            result.Should().NotBeNull();
            result.Length.Should().Be(4);
            result[0].id.Should().Be(1);
            result[1].id.Should().Be(2);
            result[2].id.Should().Be(3);
        }
        [Test]
        public async Task CapturedObject_PostAsync_GetAllAsync_Success()
        {
            var obj = new o[2]
            {
                new o()
                {
                    id = 1
                },
                new o()
                {
                    id = 2
                }
            };
            Snapshot.Out.Create<o>(2);
            await Snapshot.Out.PostAsync<o>(obj);
            var result = await Snapshot.Out.GetAllAsync<o>();

            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            result[0].id.Should().Be(1);
            result[1].id.Should().Be(2);
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
        public void CapturedObject_NullResponseFromCapturedList_Sucessfully()
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
            span.Length.Should().Be(_defaultSizeOfSnapshots);
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
            enumerable.Count().Should().Be(_defaultSizeOfSnapshots);
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
            span.Length.Should().Be(_defaultSizeOfSnapshots);
        }
    }
}
