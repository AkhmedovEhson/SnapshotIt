﻿using FluentAssertions;
using SnapshotIt.Domain.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.UnitTests
{
    public class SValueTests
    {
        [Test]
        public void GetPropertySuccessfully()
        {
            // Arrange
            var snap = new SValue<o>() { Value = new o() { id = 1 } };

            // Act
            var prop = snap.Property<int>("id");
                
            // Assert    
            prop.Should().NotBe(null);
            prop.Should().Be(1);
            
        }

        [Test]
        public void GetPropertyFail()
        {
            // Arrange
            var snap = new SValue<o> { Value = null };
            
            // Act
            var property = snap.Property<int>("unknown");

            // Assert
            property.Should().Be(null);
        }
    }
}
