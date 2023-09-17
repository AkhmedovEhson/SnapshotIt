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
        /// <seealso cref="Pick(T)"/> is method, pushes the snapshot to ( 'cache' )
        /// </summary>
        public void Pick(ref T entity)
        {       
            snapshots.Push(ref entity);
        }

        /// <summary><seealso cref="ClapOne"/> Method returns Entity if it's been snapshotted else <see cref="Exception"/></summary> 
#nullable enable
        public T? ClapOne()
        {

            return snapshots.Get();
            
        }
#nullable disable
        public void Clear()
        {
            snapshots = new Snaps<T>();
        }
    }
}
