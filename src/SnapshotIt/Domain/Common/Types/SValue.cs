using System;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SnapshotIt.Domain.Common.Types
{
    /// <summary>
    /// <seealso cref="SnapshotValue{T}"/> is result type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SnapshotValue<T>
    {
        public T Value { get; init; }

        public readonly T1 Property<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T1>(string name)
        {
            PropertyInfo propertyInfo = this.Value?.GetType().GetProperty(name) ?? throw new NullReferenceException("Property is not found");

            object property = propertyInfo.GetValue(this.Value) ?? throw new ArgumentNullException(propertyInfo.Name,$"Property ${name} is not found !");

            return (T1)property;
        }


        public readonly object? Property(string name)
        {
            PropertyInfo propertyInfo = this.Value?.GetType().GetProperty(name) ?? throw new NullReferenceException("Property is not found");

            object property = propertyInfo.GetValue(this.Value) ?? throw new ArgumentNullException(propertyInfo.Name, $"Property ${name} is not found !");

            return property;
        }
    }
}
