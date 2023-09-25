using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Common.Types
{
    /// <summary>
    /// <seealso cref="SValue{T}"/> is result type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct SValue<T>
    {
        public T Value { get; set; }

    }
}
