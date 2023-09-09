using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

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
                if (Snapshots[name].Count == 2)
                {
                    Console.WriteLine("No possible to collect more than 2 snapshots");
                    return;
                }

                Snapshots[name].Push(JsonSerializer.Serialize<T>(entity));
                return;
            }

            var stack = new Stack<string>(); 
            stack.Push(JsonSerializer.Serialize<T>(entity));
            Snapshots.Add(name, stack);

        }
        /// <summary> Method returns Entity if it's been snapshotted else <see cref="Exception"/> 
        /// <br/><i>Note: </i> Argument "action", if it's true returns current else behind snapshotted <see cref="DbSet{TEntity}"/> entity
        /// </summary>
        public static T ClapOne([AllowNull] bool? currentOrBehind = null)
        {
            string key = typeof(T).Name;
            
            if (Snapshots.ContainsKey(key) && Snapshots[key].Any())
            {
                if (currentOrBehind is false)
                {
                    Snapshots[key].Pop();
                }

                var obj = JsonSerializer.Deserialize<T>(Snapshots[key].Pop());
                return obj;
            }
            throw new Exception("No Snapshots");
        }
    }
}
