using System;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SnapshotIt.Domain.Common.Types
{
    /// <summary>
    /// <seealso cref="SValue{T}"/> is result type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SValue<T>
    {
        public T Value { get; init; }

        public readonly object Property<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T1>(string name)
        {
            PropertyInfo? propertyInfo = typeof(T1).GetProperty(name) ?? throw new NullReferenceException("Property is not found");

            object? property = propertyInfo.GetValue(this.Value) ?? throw new ArgumentNullException(propertyInfo.Name,$"Property ${name} is not found !");

            return property;
        }
    }
}
