using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SnapshotIt.DependencyInjection.UnitTests.TestObjects;
using System.Reflection;

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
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();

            var boxSingleton = dep_collection.Where(o => o.ServiceType.Name is nameof(IBox)
                                && o.Lifetime == ServiceLifetime.Singleton).FirstOrDefault();

            var boxScoped = dep_collection.Where(o => o.ServiceType.Name is nameof(IBoxScoped)
                                          && o.Lifetime == ServiceLifetime.Scoped).FirstOrDefault();

            var boxTransient = dep_collection.Where(o => o.ServiceType.Name is nameof(IBoxTransient)
                    && o.Lifetime == ServiceLifetime.Transient).FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            dep_collection.Count.Should().Be(3);

            boxSingleton.Should().NotBeNull();
            boxScoped.Should().NotBeNull();
            boxTransient.Should().NotBeNull();

        }


        [Test]
        public void GetRegisteredServiceProtectedByAttribute_Singleton_Success()
        {
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();

            var boxSingleton = dep_collection.Where(o => o.ServiceType.Name is nameof(IBox)
                              && o.Lifetime == ServiceLifetime.Singleton).FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            boxSingleton.Should().NotBeNull();
        }

        [Test]
        public void GetRegisteredServiceProtectedByAttribute_Scoped_Success()
        {
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();

            var boxScoped = dep_collection.Where(o => o.ServiceType.Name is nameof(IBoxScoped)
                              && o.Lifetime == ServiceLifetime.Scoped).FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            boxScoped.Should().NotBeNull();
        }

        [Test]
        public void GetRegisteredServiceProtectedByAttribute_Transient_Success()
        {
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();
            var boxTransient = dep_collection.Where(o => o.ServiceType.Name is nameof(IBoxTransient)
                 && o.Lifetime == ServiceLifetime.Transient).FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            boxTransient.Should().NotBeNull();
          
        }


        [Test]
        public void RegisterServiceProtectedByAttribute_Transient_WithOutServiceType_Success()
        {
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();
            var _obj = dep_collection.Where(o => o.ServiceType.Name is nameof(TestObjectWithoutServiceTypeTransient))
                .Where(o => o.Lifetime == ServiceLifetime.Transient)
                .FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            _obj.Should().NotBeNull();
            
        }

        [Test]
        public void RegisterServiceProtectedByAttribute_Scoped_WithOutServiceType_Success()
        {
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();
            var _obj = dep_collection.Where(o => o.Lifetime == ServiceLifetime.Scoped 
                && o.ServiceType.Name is nameof(TestObjectWithoutServiceTypeScoped)).FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            _obj.Should().NotBeNull();
        }

        [Test]
        public void RegisterServiceProtectedByAttribute_Singleton_WithOutServiceType_Success()
        {
            // Arrange
            var runtime = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), dep_collection);

            // Act
            runtime.ConfigureAllServices();
            var _obj = dep_collection.Where(o => o.Lifetime == ServiceLifetime.Singleton
                && o.ServiceType.Name is nameof(TestObjectWithoutServiceTypeSingleton)).FirstOrDefault();

            // Assert
            dep_collection.Should().NotBeNull();
            _obj.Should().NotBeNull();
        }
    }
}
