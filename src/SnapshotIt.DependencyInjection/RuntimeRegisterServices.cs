using Microsoft.Extensions.DependencyInjection;
using SnapshotIt.DependencyInjection.Common;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace SnapshotIt.DependencyInjection
{
    /// <summary>
    /// RuntimeRegisterServices - registers all services to dep. injection container dynamically on runtime
    /// </summary>
    public class RuntimeRegisterServices
    {
        private Assembly ExecutingAssembly { get; set; }
        private IServiceCollection ServiceCollection { get; set; }
        public RuntimeRegisterServices(Assembly assembly, IServiceCollection services)
        {
            this.ExecutingAssembly = assembly;
            this.ServiceCollection = services;
        }
        
        /// <summary>
        /// Registers service to DI container, with instance-type
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="service"></param>
        /// <param name="_interface"></param>
        private void RegisterServiceToDependencyInjectionContainer(ServiceLifetime lifetime,Type service,Type _interface)
        {
            switch(lifetime)
            {
                case ServiceLifetime.Transient:
                    ServiceCollection.AddTransient(_interface,service);
                    break;
                case ServiceLifetime.Scoped:
                    ServiceCollection.AddScoped(_interface,service);
                    break;
                default:
                    ServiceCollection.AddSingleton(_interface, service);
                    break;
            }
        }
        /// <summary>
        /// Registers service to DI container
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="service"></param>
        private void RegisterServiceToDependencyInjectionContainer(ServiceLifetime lifetime,Type service)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    ServiceCollection.AddTransient(service);
                    break;
                case ServiceLifetime.Scoped:
                    ServiceCollection.AddScoped(service);
                    break;
                default:
                    ServiceCollection.AddSingleton(service);
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void ConfigureAllServices()
        {
            IReadOnlyCollection<Type> types = ExecutingAssembly.GetExportedTypes()
                .Where(o => o.GetCustomAttribute<RuntimeDependencyInjectionOptionAttribute>() is not null)
                .ToList();

            if (types.Any())
            {
                foreach (var type in types)
                {
                    var attribute = type.GetCustomAttribute<RuntimeDependencyInjectionOptionAttribute>();
                    var _interface = type
                            .GetInterfaces()
                            .Where(o => o.Name[1..] == type.Name).FirstOrDefault();

                    if (_interface is null)
                    {
                        Console.WriteLine("`RuntimeRegisterServices` will be looking for an interface of component comparing the names, e.g. ProductService -> IProductService");
                        RegisterServiceToDependencyInjectionContainer(attribute.Lifetime, type);
                    }


                    RegisterServiceToDependencyInjectionContainer(attribute.Lifetime, type, _interface);


                }
            }
            
        }

    /// <summary>
    /// Registers services impl. `IScoped` to dependency injection container.
    /// </summary>
    public void ConfigureScopedServices()
        {
            var type = typeof(IScoped);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();
            if (types.Any())
            {
                foreach(var _type in types)
                {
                    var _interface = _type.GetInterfaces()
                        .Where(each => each.Name[1..] == _type.Name)
                        .FirstOrDefault();

                    if (_interface is null)
                    {
                        ServiceCollection.AddScoped(_type);
                        continue;
                    }
                    ServiceCollection.AddScoped(_interface, _type);
                }
            }
        }
        /// <summary>
        /// Registers services impl. `ITransient` to dependency injection container.
        /// </summary>
        public void ConfigureTransientServices()
        {
            var type = typeof(ITransient);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();
            if (types.Any())
            {
                foreach(var _type in types)
                {
                    var _interface = _type.GetInterfaces()
                      .Where(each => each.Name[1..] == _type.Name)
                      .FirstOrDefault();

                    if (_interface is null)
                    {
                        ServiceCollection.AddTransient(_type);
                        continue;
                    }

                    ServiceCollection.AddTransient(_interface, _type);
                }
            }
        }
        /// <summary>
        /// Registers services impl. `ISingleton` to dependency injection container.
        /// </summary>
        public void ConfigureSingletonServices()
        {
            var type = typeof(ISingleton);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();
            if (types.Any())
            {
                foreach(var _type in types)
                {
                    var _interface = _type.GetInterfaces()
                      .Where(each => each.Name[1..] == _type.Name)
                      .FirstOrDefault();

                    if (_interface is null)
                    {
                        ServiceCollection.AddSingleton(_type);
                        continue;
                    }
                    ServiceCollection.AddSingleton(_interface,_type);
                }
            }
        }

    }
}

