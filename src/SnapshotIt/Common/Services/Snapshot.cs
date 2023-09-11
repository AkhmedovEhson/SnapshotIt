using System.Text.Json;

namespace SnapshotIt.Common.Services
{
    public static class Snap<T> where T : class
    {
        private static IDictionary<string, Stack<string>> Snapshots  = new Dictionary<string, Stack<string>>();

        public static void Pick(T entity)
        {           
            string name = entity.GetType().Name;
            if (Snapshots.ContainsKey(name))
            {
                Snapshots[name].Pop();
                Snapshots[name].Push(JsonSerializer.Serialize<T>(entity));
                return;
            }

            var stack = new Stack<string>(); 
            stack.Push(JsonSerializer.Serialize<T>(entity));
            Snapshots.Add(name, stack);

        }

        /// <summary> Method returns Entity if it's been snapshotted else <see cref="Exception"/></summary> 
        public static T ClapOne()
        {
            string key = typeof(T).Name;
            
            if (Snapshots.ContainsKey(key))
            {
                return JsonSerializer.Deserialize<T>(Snapshots[key].Pop())!;
            }
            throw new Exception("No Snapshots");
        }

        public static void Clear()
        {
            Snapshots.Clear();
        }
    }
}
