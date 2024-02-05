using Microsoft.Extensions.DependencyInjection;
using SnapshotIt.DependencyInjection.ThirdParties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection
{
    /// <summary>
    /// RuntimeRegisterServices - registers all services to dep. injection container dynamically on runtime
    /// </summary>
    public class RuntimeRegisterServices
    {
        private Assembly ExecutingAssembly { get; set; }
        private IServiceCollection ServiceCollection { get; set; }
        public RuntimeRegisterServices(Assembly assembly,IServiceCollection services)
        {
            this.ExecutingAssembly = assembly;
            this.ServiceCollection = services;
        }

        /// <summary>
        /// Registers all services impl. `IRuntimeDependencyInjectionObject` to dep. injection container.
        /// </summary>
        /// <param name="store"></param>
        public void ConfigureAllServices(ServiceLifetime lifetime)
        {
            var type = typeof(IRuntimeDependencyInjectionObject);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();

            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    {
                        if (types.Any())
                        {
                            foreach (var _type in types)
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
                        break;
                    }
                case ServiceLifetime.Scoped:
                    { 
                        if (types.Any())
                        {
                            foreach (var _type in types)
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
                        break;
                    }
                default:
                    {
                        if (types.Any())
                        {
                            foreach (var _type in types)
                            {
                                var _interface = _type.GetInterfaces()
                                  .Where(each => each.Name[1..] == _type.Name)
                                  .FirstOrDefault();

                                if (_interface is null)
                                {
                                    ServiceCollection.AddSingleton(_type);
                                    continue;
                                }
                                ServiceCollection.AddSingleton(_interface, _type);
                            }
                        }
                        break;
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
