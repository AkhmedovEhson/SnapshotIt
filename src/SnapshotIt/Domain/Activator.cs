using SnapshotIt.Domain.Common.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace SnapshotIt.Domain;

internal static class ActivatorIt
{
    public static T CreateInstanceWithCopyingProperties
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>(T instance)
    {
        var copy = Activator.CreateInstance<T>();
        PropertyReflection.SetProperties(instance, ref copy);
        return copy;
    }
}
