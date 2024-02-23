
using Microsoft.Extensions.DependencyInjection;
using SnapshotIt.DependencyInjection.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.UnitTests.TestObjects
{
    [RuntimeDependencyInjectionOption(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class Box:IBox { }
    public interface IBox { }

    [RuntimeDependencyInjectionOption(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    public class BoxScoped:IBoxScoped { }
    public interface IBoxScoped { }

    [RuntimeDependencyInjectionOption(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
    public class BoxTransient:IBoxTransient { }
    public interface IBoxTransient { }


    // ---------------------------- OBJECTS WTIHOUT INSTANCE-TYPE ---------------------------- //

    [RuntimeDependencyInjectionOption(Lifetime = ServiceLifetime.Singleton)]
    public class TestObjectWithoutServiceTypeSingleton { }

    [RuntimeDependencyInjectionOption(Lifetime = ServiceLifetime.Scoped)]
    public class TestObjectWithoutServiceTypeScoped { }

    [RuntimeDependencyInjectionOption(Lifetime = ServiceLifetime.Transient)]
    public class TestObjectWithoutServiceTypeTransient { }
}
