using Microsoft.Extensions.DependencyInjection;
using SnapshotIt.DependencyInjection.Common;
using System.Reflection;

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

        private void RegisterServiceToDependencyInjectionContainer(ServiceLifetime lifetime,Type service,Type _interface)
        {
            switch(lifetime)
            {
                case ServiceLifetime.Transient:
                    ServiceCollection.AddTransient(service, _interface);
                    break;
                case ServiceLifetime.Scoped:
                    ServiceCollection.AddScoped(service, _interface);
                    break;
                default:
                    ServiceCollection.AddSingleton(service, _interface);
                    break;
            }
        }

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

        public void ConfigureAllServices()
        {

            var attributes = ExecutingAssembly.GetCustomAttributes<RuntimeDependencyInjectionOptionAttribute>();
            var types = ExecutingAssembly.GetTypes();

            var list = new List<ComponentProtectedByAttributeResponse>();

            if (types.Any())
            {
                foreach(var type in types)
                {
                    var customAttribute = type.GetCustomAttribute<RuntimeDependencyInjectionOptionAttribute>();
                    if (customAttribute != null)
                    {
                        list.Add(
                            new ComponentProtectedByAttributeResponse()
                            {
                                ServiceLifetime = customAttribute.Lifetime,
                                Type = type,
                            });
                        continue;
                    }
                }
            }

            if (list.Any())
            {
                foreach(var item in list)
                {
                    var _interface = item.Type.GetType()
                        .GetInterfaces()
                        .Where(o => item.Type.Name == o.Name[1..]).FirstOrDefault();


                    if (_interface != null)
                    {
                        RegisterServiceToDependencyInjectionContainer(item.ServiceLifetime, item.Type,_interface);
                        continue;
                    }

                    RegisterServiceToDependencyInjectionContainer(item.ServiceLifetime, item.Type);
                    
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
