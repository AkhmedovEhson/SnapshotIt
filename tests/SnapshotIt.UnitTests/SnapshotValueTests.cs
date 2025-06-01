using FluentAssertions;
using SnapshotIt.Domain.Common.Types;
using SnapshotIt.UnitTests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.UnitTests
{
    public class SnapshotValueTests
    {
        [Test]
        public void GetProperty_Found()
        {
            // Arrange
            var snap = new SnapshotValue<o>() { Value = new o() { id = 1 } };

            // Act
            var prop = snap.Property<int>("id");
                
            // Assert    
            prop.Should().NotBe(null);
            prop.Should().Be(1);
            
        }

        [Test]
        public void GetProperty_NotFound_ThrowsNullReferenceException()
        {
            // Arrange
            var snap = new SnapshotValue<o> { Value = null };
            

            // Act && Assert
            Assert.Throws<NullReferenceException>(() => snap.Property<int>("property"));
        }

        [Test]
        public void GetProperty_WrongCastType_ThrowsInvalidCastException()
        {
            var snap = new SnapshotValue<o> { Value = new o() { id = 1 } };

            // Act && Assert
            Assert.Throws<InvalidCastException>(() => snap.Property<string>("id"));
        }
    }
}
