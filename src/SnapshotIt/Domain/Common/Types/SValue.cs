using System;

using System.Diagnostics.CodeAnalysis;

namespace SnapshotIt.Domain.Common.Types
{
    /// <summary>
    /// <seealso cref="SValue{T}"/> is result type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SValue<T>
    {
        public T Value { get; set; }

        public readonly object Property<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T1>(string name)
        {
            Type _ = typeof(T);

            object? property = _.GetProperty(name)?.GetValue(this.Value) ?? throw new ArgumentNullException(_.Name,$"Property ${name} is not found !");

            return property;
        }
    }
}
