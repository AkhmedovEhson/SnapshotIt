using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SnapshotIt.DependencyInjection.UnitTests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.UnitTests
{
    public class RuntimeRegisterServicesTests
    {
        private ServiceCollection dep_collection = new ServiceCollection();
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(),dep_collection);
            runtime.ConfigureAllServices(Store.Transient);
        }

        [Test]
        public void GetRegisteredServicesInDICollection_Success()
        {
            dep_collection.Should().NotBeNull();

            // Note: should be `1` because there is only one class impl. SnapshotIt's interface on this assembly 
            dep_collection.Count.Should().Be(1); 
            dep_collection[0].Lifetime.Should().Be(ServiceLifetime.Transient);
        }
    }
}
