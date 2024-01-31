﻿using FluentAssertions;
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
        [SetUp]
        public void RunBeforeAnyTests()
        {
            dep_collection.Clear();
        }

        [Test]
        public void GetRegisteredServicesInDICollection_Success()
        {
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);
            runtime.ConfigureAllServices(Store.Singleton);
            dep_collection.Should().NotBeNull();

            // Note: should be `1` because there is only one class impl. SnapshotIt's interface on this assembly 
            dep_collection.Count.Should().Be(1); 
            dep_collection[0].Lifetime.Should().Be(ServiceLifetime.Transient);
        }

        [Test]
        public void GetRegisterdServices_Transient_Success()
        {
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);
            runtime.ConfigureTransientServices();
            dep_collection.Should().NotBeNull();
            dep_collection[0].Lifetime.Should().Be(ServiceLifetime.Transient);
        }


        [Test]
        public void GetRegisteredServices_Scoped_Success()
        {
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);
            runtime.ConfigureScopedServices();
            dep_collection.Should().NotBeNull();
            dep_collection[0].Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [Test]
        public void GetRegisteredServices_Singleton_Success()
        {
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);
            runtime.ConfigureSingletonServices();
            dep_collection.Should().NotBeNull();
            dep_collection[0].Lifetime.Should().Be(ServiceLifetime.Singleton);
        }
    }
}
