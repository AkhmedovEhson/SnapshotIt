using SnapshotIt.Domain.Utils;
using SnapshotIt.Domain.Common.Types;
namespace SnapshotIt.Common.Services
{

    /// <summary> <seealso cref="Snap{T}"/> is mini-util, provides snapshot action </summary>
    public class Snap<T> where T : class
    {
        private Snaps<T> snapshots = new Snaps<T>();
        public T[] snapshot_collection => snapshots.captures;
        /// <summary>
        /// <seealso cref="Pick(T)"/> pushes the snapshot to `heap`
        /// </summary>
        public void Pick(T entity)
        {       
            snapshots.Push(in entity);
        }

        /// <summary><seealso cref="ClapOne"/> gets snapshot</summary> 
#nullable enable
        public SValue<T?> ClapOne()
        {
            return snapshots.Get();           
        }

        public SValue<T?> ClapOne(int idx)
        {
            return snapshots.Get(idx);
        }
#nullable disable


        /// <summary> <seealso cref="Clear"/> clears snapshots - history </summary>
        public void Clear()
        {
            snapshots = new Snaps<T>();
        }
        public Snap():this(null)
        {

        }
        public Snap(T entity)
        {
            snapshots.Push(in entity);
            
        }
    }
}
