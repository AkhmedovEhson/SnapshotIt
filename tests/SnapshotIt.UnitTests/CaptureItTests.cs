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
        public void RunBeforeAllTests()
        { 
           
            Snapshot.Out.Create<o>(_defaultSizeOfSnapshots);
        }

        [Test]
        public void Post_Successfully()
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
        public void Clear_ClearDefinedType_Successfully()
        {
            Assert.DoesNotThrow(() =>
            {
                Snapshot.Out.Get<o>(0);
            });

            Snapshot.Out.Clear<o>();

            Assert.Throws<NullReferenceException>(() =>
            {
                Snapshot.Out.Get<o>(0);
            });
        }

        [Test]
        public void Clear_ClearNotFoundType_Throws_ArgumentNullReferenceException()
        {
        
            Assert.Throws<ArgumentNullException>(() =>
            {
                Snapshot.Out.Clear<int>();
            });
        }
      
        [Test]
        public void ClearAll_Successfully()
        {
            Snapshot.Out.Create<decimal>(10);

            for(int i = 0; i < 10; i++)
            {
                Snapshot.Out.Post<decimal>(i);
            }

            Snapshot.Out.Create<short>(20);

            for (int i = 0; i < 20; i++)
            {
                Snapshot.Out.Post<short>((short)i);
            }

            Snapshot.Out.ClearAll(); // WIP: Clears all sets ...

            Assert.Throws<NullReferenceException>(() =>
            {
                Snapshot.Out.Get<decimal>(0);
            });

      
            Assert.Throws<NullReferenceException>(() =>
            {
                Snapshot.Out.Get<short>(0);
            });


            Assert.Throws<NullReferenceException>(() =>
            {
                Snapshot.Out.Get<o>(0);
            });
        }

        [Test]
        public async Task GetAsync_Full_Success()
        {
            // Arrange
            var @snapshot = new o[1] 
            {
                new()
                {
                    id = 1
                } 
            };
  
            // Act
            await Snapshot.Out.PostAsync<o>(snapshot);
            var @object = await Snapshot.Out.GetAsync<o>(0);

            @object.Should().NotBeNull();
            @object.id.Should().Be(1);

            // Assert
            var other_objects = Snapshot.Out.GetAsList<o>()[1..].AsEnumerable();
            other_objects.Should().AllSatisfy(e =>
            {
                e.Should().BeNull();
            });
                       
        }

        [Test]
        public async Task GetAsync_Success()
        {
            // Arrange
            var @snapshot = new o[1]
            {
                new()
                {
                    id = 1
                }
            };

            // Act
            await Snapshot.Out.PostAsync<o>(snapshot);
            var @object = await Snapshot.Out.GetAsync<o>(0);

            // Assert
            @object.Should().NotBeNull();
            @object.id.Should().Be(1);
        }

        [Test]
        public async Task PostAsync_GetAllAsync_ArrayTimesToTwo_Success()
        {
            // Arrange
            var obj = new int[4]
            {
                1,2,3,4
            };

            Snapshot.Out.Create<int>(3);
            var snapshots = await Snapshot.Out.GetAllAsync<int>();

            snapshots.Should().NotBeNull();
            snapshots.Length.Should().Be(3);

            // Act
            await Snapshot.Out.PostAsync<int>(obj);
            var result = await Snapshot.Out.GetAllAsync<int>();

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().Be(6);

        }
        [Test]
        public async Task PostAsync_GetAllAsync_Success()
        {
            // Arrange
            var snapshots = new int[2]
            {
                1,2
            };
            Snapshot.Out.Create<int>(2);

            // Act
            await Snapshot.Out.PostAsync<int>(snapshots);
            var result = await Snapshot.Out.GetAllAsync<int>();

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            result[0].Should().Be(1);
            result[1].Should().Be(2);
        }

        [Test]
        public void SnapshotNotFound_Throws_NullReferenceException_Expression()
        {
            // Arrange && Act
            Snapshot.Out.Post<o>(new o() { id = 1});

            // Assert
            Assert.Throws<NullReferenceException>(() => Snapshot.Out.Get<o>(i => i.id == 2));
        }

        [Test]
        public void SnapshotFound_Expression_Success()
        {
            // Arrange
            Snapshot.Out.Post<o>(new o() { id = 1 });

            // Act
            var result = Snapshot.Out.Get<o>(i => i.id == 1);

            // Assert
            result.Should().NotBeNull();
            result.id.Should().Be(1);
        }

        [Test]
        public void NotFound_ByIndex_RespondNull()
        {
            // Arrange && Act
            var result = Snapshot.Out.Get<o>(0);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void SnapshotByIndex_Throws_IndexOutOfRangeException()
        {
            // Arrange
            Snapshot.Out.Create<int>(1);
            var @enumerable = Snapshot.Out.GetAsEnumerable<int>();
            @enumerable.Count().Should().Be(1);

            // Act
            Snapshot.Out.Post(1);

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => {
                var item = Snapshot.Out.Get<int>(3); // out of size
            });
        }

        [Test]
        public void Snapshots_GetAsSpan_Successfully()
        {
            // Arrange && Act
            var span = Snapshot.Out.GetAsSpan<o>();


            // Assert
            bool result = span is Span<o>;
            result.Should().BeTrue();
            span.Length.Should().Be(_defaultSizeOfSnapshots);
        }

        [Test]
        public void Snapshots_GetAsEnumerable_Successfully()
        {
            // Arrange && Act
            var enumerable = Snapshot.Out.GetAsEnumerable<o>();

            // Assert
            Assert.IsInstanceOf<IEnumerable<o>>(enumerable);
            enumerable.Count().Should().Be(_defaultSizeOfSnapshots);
        }

        [Test]
        public void Snapshots_GetAsReadonlySpan_Successfully()
        {
            // Arrange && Act
            var readonly_span = Snapshot.Out.GetAsReadonlySpan<o>();

            // Assert
            bool result = readonly_span is ReadOnlySpan<o>;
            result.Should().BeTrue();
            readonly_span.Length.Should().Be(_defaultSizeOfSnapshots);
        }
    }
}
