using System.Collections.Concurrent;
using System.Text.Json;

namespace SnapshotIt.Common.Services
{
    /// <summary> <seealso cref="Snap{T}"/> is mini-util, provides snapshot action </summary>
    public static class Snap<T> where T : class
    {
        private static ConcurrentDictionary<string, Stack<string>> Snapshots  = new ConcurrentDictionary<string, Stack<string>>();

        /// <summary>
        /// <seealso cref="Pick(T)"/> is method, pushes the snapshot to ( 'cache' )
        /// </summary>
        public static void Pick(T entity)
        {           
            string name = entity.GetType().Name;
            if (Snapshots.ContainsKey(name))
            {
                Snapshots.GetValueOrDefault(name)?.Pop();
                Snapshots.GetValueOrDefault(name)?.Push(JsonSerializer.Serialize<T>(entity));
                return;
            }

            var stack = new Stack<string>(); 
            stack.Push(JsonSerializer.Serialize<T>(entity));
            Snapshots.TryAdd(name, stack);
        }

        /// <summary><seealso cref="ClapOne"/> Method returns Entity if it's been snapshotted else <see cref="Exception"/></summary> 
        public static T ClapOne()
        {
            string key = typeof(T).Name;
            
            if (Snapshots.ContainsKey(key))
            {
                return JsonSerializer.Deserialize<T>(Snapshots.GetValueOrDefault(key)!.Pop())!;
            }
            throw new Exception("No Snapshots");
        }

        public static void Clear()
        {
            Snapshots.Clear();
        }
    }
}
