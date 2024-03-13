using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public readonly object Property<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T1>(string name)
        {
            Type _ = typeof(T);
            return _.GetProperty(name)?.GetValue(this.Value);
        }
    }
}
