using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace SnapshotIt.DependencyInjection.UnitTests
{
    public class ConnectorTests
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ProductTests>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Configured all DI services
            Connector.ConfigureConnector(serviceProvider); 

            var product = serviceProvider.GetRequiredService<ProductTests>();
            product.Id = 1;
            product.Name = "Product";
        }

        [Test]
        public void GetService_SuccessByGenericType()
        {
            var service = Connector.GetService<ProductTests>();
            service.Should().NotBeNull();  
            service.Should().BeOfType<ProductTests>();
            service.Id.Should().Be(1);
            service.Name.Should().Be("Product");
        }

        [Test]
        public void GetService_SuccessByTypeFromArgument()
        {
            var service = Connector.GetService(typeof(ProductTests));
            service.Should().NotBeNull();

            var product = (ProductTests?)service;

            product.Id.Should().Be(1);
            product.Name.Should().Be("Product");
        }


        [Test]
        public void GetService_FailByTypeFromArgument()
        {
            var service = Connector.GetService(typeof(object));
            service.Should().BeNull();
        }

        [Test]
        public void GetService_FailByGenericType()
        {
            var service = Connector.GetService<object>();
            service.Should().BeNull();
        }


    }
}