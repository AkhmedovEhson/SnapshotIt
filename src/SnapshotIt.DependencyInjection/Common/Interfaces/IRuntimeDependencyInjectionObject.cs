using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection
{
    /// <summary>
    /// IRuntimeDependencyInjectionObject - common gateway to DI container
    /// </summary>
    public interface IRuntimeDependencyInjectionObject { }
    /// <summary>
    /// IScoped - gateway with lifetime `Scoped` to DI container
    /// </summary>
    public interface IScoped { }
    /// <summary>
    /// ITransient - gateway with lifetime `Transient` to DI container
    /// </summary>
    public interface ITransient { }
    /// <summary>
    /// ISingleton - gateway with lifetime `Singleton` to DI container
    /// </summary>
    public interface ISingleton { }
}
