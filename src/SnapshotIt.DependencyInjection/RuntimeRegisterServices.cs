using Microsoft.Extensions.DependencyInjection;
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
        public void ConfigureAllServices()
        {
            var singleton = typeof(IRuntimeDependencyInjectionObject<ISingleton>);
            var transient = typeof(IRuntimeDependencyInjectionObject<ITransient>);
            var scoped = typeof(IRuntimeDependencyInjectionObject<IScoped>);
            bool _lifetime = false;
            var types = ExecutingAssembly.GetExportedTypes()
                .Where(o => o.IsClass)
                .Where(o => singleton.IsAssignableFrom(o) || transient.IsAssignableFrom(o) || scoped.IsAssignableFrom(o))
                .ToList();

            ServiceLifetime lifetime = default;

            if (!types.Any())
                return;

            foreach (var _type in types)
            {
                Type? _interface = default;

                var _interfaces = _type.GetInterfaces().AsEnumerable();
                // Note: instead of scripting ( O(n^3 e.g or more ...) ), uses `foreach` loop
                foreach (var item in _interfaces)
                {
                    if (item.Name[1..] == _type.Name)
                    {
                        _interface = item;
                        continue;
                    }


                    if (_lifetime == false)
                    {
                        if (item.IsEquivalentTo(singleton))
                        {
                            lifetime = ServiceLifetime.Singleton;
                        }
                        else if (item.IsEquivalentTo(transient))
                        {
                            lifetime = ServiceLifetime.Transient;
                        }
                        else if (item.IsEquivalentTo(scoped))
                        {
                            lifetime = ServiceLifetime.Scoped;
                        }
                        _lifetime = true;
                    }

                }

                    
                if (_lifetime)
                {
                    _lifetime = false;
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            {
                                if (_interface is not null)
                                {
                                    ServiceCollection.AddSingleton(_interface, _type);
                                    break;
                                }
                                ServiceCollection.AddSingleton(_type);
                                break;
                            }
                        case ServiceLifetime.Transient:
                            {
                                if (_interface is not null)
                                {
                                    ServiceCollection.AddTransient(_interface, _type);
                                    break;
                                }
                                ServiceCollection.AddTransient(_type);
                                break;
                            }
                        case ServiceLifetime.Scoped:
                            {
                                if (_interface is not null)
                                {
                                    ServiceCollection.AddScoped(_interface, _type);
                                    break;
                                }
                                ServiceCollection.AddScoped(_type);
                                break;
                            }
                    }
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
