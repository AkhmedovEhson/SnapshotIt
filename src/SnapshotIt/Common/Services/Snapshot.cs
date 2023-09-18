using SnapshotIt.Domain.Utils;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SnapshotIt.Common.Services
{

    /// <summary> <seealso cref="Snap{T}"/> is mini-util, provides snapshot action </summary>
    public class Snap<T> where T : class
    {
        private Snaps<T> snapshots = new Snaps<T>();

        /// <summary>
        /// <seealso cref="Pick(T)"/> pushes the snapshot to `heap`
        /// </summary>
        public void Pick(in T entity)
        {       
            snapshots.Push(in entity);
        }

        /// <summary><seealso cref="ClapOne"/> gets snapshot</summary>
#nullable enable
        public T? ClapOne()
        {

            
            return snapshots.Get();
        }
#nullable disable
        /// <summary> <seealso cref="Clear"/> clears snapshots - history </summary>
        public void Clear()
        {
            snapshots = new Snaps<T>();
        }
        public Snap(in T entity)
        {
            snapshots.Push(in entity);
        }
    }
}
