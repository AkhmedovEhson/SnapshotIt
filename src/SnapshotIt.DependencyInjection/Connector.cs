
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace SnapshotIt.DependencyInjection
{
    /// <summary>
    /// Connector - has direct line to DI, it helps to get rid of injections from constructor 
    /// </summary>
    public static class Connector
    {
        private static IServiceProvider _serviceProvider;
        private static Assembly _executingAssembly;

        /// <summary>
        /// ConfigureConnector - adjusts _service-provider, and will use it in runtime of application
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void ConfigureConnector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider; 
        }

        /// <summary>
        /// ConfigureConnector - adjusts $_service-provider, and $executing-assembly and will use it in runtime of application
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void ConfigureConnector(Assembly assembly,IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _executingAssembly = assembly;
        }

        /// <summary>
        /// GetService - gets service from DI container, but uses $_service-provider
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetService<T>()
        {
            if (_serviceProvider is null)
            {
                throw new ArgumentNullException($"Provided `$service-provider` is not found");
            }

            return _serviceProvider.GetService<T>() ?? throw new ArgumentNullException("Service is not found in dep. injection container");
        }

        /// <summary>
        /// GetService - gets service from DI container, but uses $_service-provider
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetService(Type serviceType)
        {
            if (_serviceProvider is null)
            {
                throw new ArgumentNullException($"Provided `$service-provider` is not found");
            }

            return _serviceProvider.GetService(serviceType) ?? throw new ArgumentNullException("Service is not found in dep. injection container"); ;
        }
        /// <summary>
        /// CreateScope - creates scope synchronously.
        /// </summary>
        /// <returns></returns>
        public static IServiceScope CreateScope()
        {
            return _serviceProvider.CreateScope();
        }
        /// <summary>
        /// CreateScopeAsync - creates scope asynchronously
        /// </summary>
        /// <returns></returns>
        public static AsyncServiceScope CreateScopeAsync()
        {
            return _serviceProvider.CreateAsyncScope();
        }
    }
}
