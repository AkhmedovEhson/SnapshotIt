using SnapshotIt.Domain.Common.Reflection;

namespace SnapshotIt.Domain;

public static class ActivatorIt
{
    public static T CreateInstanceWithCopyingProperties<T>(T instance)
    {
        var copy = Activator.CreateInstance<T>();
        PropertyReflection.SetProperties(instance, ref copy);
        return copy;
    }
}
