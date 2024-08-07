using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace SnapshotIt.DependencyInjection.UnitTests
{
    public class ConnectorTests
    {
        [OneTimeSetUp]
        public void RunOnetimeBeforeRunningTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ProductTests>();
            serviceCollection.AddTransient<ColourTests>();
            serviceCollection.AddScoped<ColourTestsAsScoped>();
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
            service.Should().BeOfType<ProductTests>();
            service.Id.Should().Be(1);
            service.Name.Should().Be("Product");
        }

        [Test]
        public void GetService_SuccessByTypeFromArgument()
        {
            var service = Connector.GetService(typeof(ProductTests));
            var product = (ProductTests)service;
            product.Id.Should().Be(1);
            product.Name.Should().Be("Product");
        }


        [Test]
        public void GetService_FailByTypeFromArgument()
        {
            Assert.Throws<ArgumentNullException>(() => Connector.GetService(typeof(object)));
        }

        [Test]
        public void GetService_FailByGenericType()
        {
            Assert.Throws<ArgumentNullException>(() => Connector.GetService<object>());
        }

        [Test]
        public void GetServiceAsTransient_Success()
        {
            var service_transient = Connector.GetService<ColourTests>();

            service_transient.Id = 1;
            service_transient.Name = "colour";

            var service_transient_secondAttempt = Connector.GetService<ColourTests>();

            service_transient_secondAttempt.Id.Should().NotBe(1);
            service_transient_secondAttempt.Name.Should().NotBe("colour");

        }

        [Test]
        public void GetServiceAsScoped_Success()
        {
            var service_scoped = Connector.GetService<ColourTestsAsScoped>();

            service_scoped.Id = 1;
            service_scoped.Name = "colour";

            var service_scoped_secondAttempt = Connector.GetService<ColourTestsAsScoped>();

            service_scoped_secondAttempt.Id.Should().Be(1);
            service_scoped_secondAttempt.Name.Should().Be("colour");
        }

        [Test]
        public void GetServiceAsSingleton_Success()
        {
            var service_singleton = Connector.GetService<ProductTests>();

            service_singleton.Id = 1;
            service_singleton.Name = "colour";

            var service_singleton_secondAttempt = Connector.GetService<ProductTests>();

            service_singleton_secondAttempt.Id.Should().Be(1);
            service_singleton_secondAttempt.Name.Should().Be("colour");
        }

        [Test]
        public void CreateScope_Success()
        {
            using var scope = Connector.CreateScope();

            scope.Should().NotBeNull();
            scope.ServiceProvider.Should().NotBeNull();

            scope.ServiceProvider.GetService<ProductTests>().Should().NotBeNull();
            scope.ServiceProvider.GetService<ColourTests>().Should().NotBeNull();
            scope.ServiceProvider.GetService<ColourTestsAsScoped>().Should().NotBeNull();   
        }

        [Test]
        public void CreateScopeAsync_Success()
        {
            using var scope = Connector.CreateScopeAsync();

            scope.Should().NotBeNull(); 
            scope.ServiceProvider.Should().NotBeNull();


            scope.ServiceProvider.GetService<ProductTests>().Should().NotBeNull();
            scope.ServiceProvider.GetService<ColourTests>().Should().NotBeNull();
            scope.ServiceProvider.GetService<ColourTestsAsScoped>().Should().NotBeNull();
        }

    }
}
