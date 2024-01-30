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

            types.ForEach(o => ServiceCollection.AddScoped(o));
        }

        public void ConfigureTransientServices()
        {
            var type = typeof(ITransient);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();

            types.ForEach(o => ServiceCollection.AddTransient(o));
        }

        public void ConfigureSingletonServices()
        {
            var type = typeof(ISingleton);
            var types = ExecutingAssembly.GetExportedTypes().Where(o => type.IsAssignableFrom(o) && o.IsClass).ToList();

            types.ForEach(o => ServiceCollection.AddTransient(o));
        }

    }
}
