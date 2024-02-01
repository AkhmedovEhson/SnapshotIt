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
            var service = Connector.GetService<ProductTests>() ?? throw new NullReferenceException();
            service.Should().NotBeNull();  
            service.Should().BeOfType<ProductTests>();
            service.Id.Should().Be(1);
            service.Name.Should().Be("Product");
        }

        [Test]
        public void GetService_SuccessByTypeFromArgument()
        {
            var service = Connector.GetService(typeof(ProductTests)) ?? throw new NullReferenceException();
            service.Should().NotBeNull();

            var product = (ProductTests?)service ?? throw new NullReferenceException();

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

        [Test]
        public void GetServiceAsTransient_Success()
        {
            var service_transient = Connector.GetService<ColourTests>() ?? throw new NullReferenceException();
            service_transient.Should().NotBeNull();

            service_transient.Id = 1;
            service_transient.Name = "colour";

            var service_transient_secondAttempt = Connector.GetService<ColourTests>() ?? throw new NullReferenceException();
            service_transient_secondAttempt.Should().NotBeNull();

            service_transient_secondAttempt.Id.Should().NotBe(1);
            service_transient_secondAttempt.Name.Should().NotBe("colour");

        }

        [Test]
        public void GetServiceAsScoped_Success()
        {
            var service_scoped = Connector.GetService<ColourTestsAsScoped>() ?? throw new NullReferenceException();
            service_scoped.Should().NotBeNull();

            service_scoped.Id = 1;
            service_scoped.Name = "colour";

            var service_scoped_secondAttempt = Connector.GetService<ColourTestsAsScoped>() ?? throw new NullReferenceException();
            service_scoped_secondAttempt.Should().NotBeNull();

            service_scoped_secondAttempt.Id.Should().Be(1);
            service_scoped_secondAttempt.Name.Should().Be("colour");
        }

        [Test]
        public void GetServiceAsSingleton_Success()
        {
            var service_singleton = Connector.GetService<ProductTests>() ?? throw new NullReferenceException();
            service_singleton.Should().NotBeNull();

            service_singleton.Id = 1;
            service_singleton.Name = "colour";

            var service_singleton_secondAttempt = Connector.GetService<ProductTests>() ?? throw new NullReferenceException();
            service_singleton_secondAttempt.Should().NotBeNull();

            service_singleton_secondAttempt.Id.Should().Be(1);
            service_singleton_secondAttempt.Name.Should().Be("colour");
        }

        [Test]
        public void CreateScope_Success()
        {
            var scope = Connector.CreateScope();
            scope.Should().NotBeNull();
            scope.ServiceProvider.Should().NotBeNull();
        }

        [Test]
        public void CreateScopeAsync_Success()
        {
            var scope = Connector.CreateScopeAsync();
            scope.Should().NotBeNull(); 
            scope.ServiceProvider.Should().NotBeNull();
        }

    }
}