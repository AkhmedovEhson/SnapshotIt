using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection
{
    public class RuntimeRegisterServices
    {
        private Assembly ExecutingAssembly { get; set; }
        private IServiceCollection ServiceCollection { get; set; }
        public RuntimeRegisterServices(Assembly assembly,IServiceCollection services)
        {
            this.ExecutingAssembly = assembly;
            this.ServiceCollection = services;
        }

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

        public void ConfigureScopedServices()
        {
            var type = typeof(IScoped);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();

            if (types.Any())
            {
                foreach(var _type in types)
                {
                    var _interface = _type.GetInterfaces()[0];

                    if (_interface.FullName == "IScoped")
                    {
                        ServiceCollection.AddScoped(_type);
                        continue;
                    }

                    ServiceCollection.AddScoped(_interface, _type);
                }
            }
        }

        public void ConfigureTransientServices()
        {
            var type = typeof(ITransient);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();

            if (types.Any())
            {
                foreach(var _type in types)
                {
                    var _interface = _type.GetInterfaces()[0];

                    if (_interface.FullName == "ITransient")
                    {
                        ServiceCollection.AddTransient(_type);
                        continue;
                    }

                    ServiceCollection.AddTransient(_interface, _type);
                }
            }
        }

        public void ConfigureSingletonServices()
        {
            var type = typeof(ISingleton);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();
            if (types.Any())
            {
                foreach(var _type in types)
                {
                    var _interface = _type.GetInterfaces()[0];

                    if (_interface.FullName == "ISingleton")
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
