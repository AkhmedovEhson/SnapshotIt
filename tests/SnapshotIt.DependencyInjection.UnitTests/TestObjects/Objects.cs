
using SnapshotIt.DependencyInjection.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.UnitTests.TestObjects
{
    public class Car:IRuntimeDependencyInjectionObject<IScoped>
    {
        public int Id { get; set; } 
    }
    public class Car2 : IRuntimeDependencyInjectionObject<ITransient>
    {
        public int Id { get; set; } 
    }
    public class Car3 : IRuntimeDependencyInjectionObject<ISingleton>
    {
        public int Id { get; set; }
    }

    public class A : IA,IScoped { }
    public interface IA { }
    public class B : ITransient { }
    public class  C : ISingleton { }


    // --------------------------------------
    [RuntimeDependencyInjectionOption(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class Box:IBox
    {
        public int Id { get; set; }
    }
    public interface IBox { }

    [RuntimeDependencyInjectionOption(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    public class BoxScoped:IBoxScoped { }
    public interface IBoxScoped { }

    [RuntimeDependencyInjectionOption(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
    public class BoxTransient:IBoxTransient { }
    public interface IBoxTransient { }
}
