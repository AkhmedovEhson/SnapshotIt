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
        public void ConfigureAllServices(Store store)
        {
            var type = typeof(IRuntimeDependencyInjectionObject);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();

            switch (store)
            {
                case Store.Transient:
                    types.ForEach(o => ServiceCollection.AddTransient(o));
                    break;
                case Store.Scoped:
                    types.ForEach(o => ServiceCollection.AddScoped(o));
                    break;
                default:
                    types.ForEach(o => ServiceCollection.AddSingleton(o));
                    break;

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
                    var _interface = _type.GetInterfaces()[0];

                    if (_interface.Name == "IScoped")
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
                    var _interface = _type.GetInterfaces()[0];

                    if (_interface.Name == "ITransient")
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
                    var _interface = _type.GetInterfaces()[0];

                    if (_interface.Name == "ISingleton")
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
