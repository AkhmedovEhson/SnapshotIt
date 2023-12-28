using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{
    public static class CaptureIt<T>
    {
        private static T[] collection = null;
        private static int index = 0;
        private static int size = 0;

        public static T Get(int ind)
        {
            if (ind < 0 || ind >= collection.Length)
            {
                throw new IndexOutOfRangeException($"Index is out of range, input ( {typeof(T).FullName} )");
            }
 
            return collection[ind];
        }

        public static void Create(int s)
        {
            collection = new T[s];
            size = s;
        }

        /// <summary>
        /// Copies value and pastes in collection
        /// </summary>
        /// <param name="value"></param>
        public static void Post(T value)
        {
            if (value is ValueType)
            {
                collection[index == collection.Length - 1 ? index : index++] = value;
                return;
            }

            var instance = Snapshot.Out.Copy<T>(value);

            collection[index == collection.Length - 1 ? index : index++] = instance;
        }
        /// <summary>
        /// Resets collection, makes `collection` for pointing to null
        /// </summary>
        /// <param name="s"></param>
        public static void Reset(int? s)
        {
            collection = new T[!s.HasValue ? size : s.Value];
        }

        public static Span<T> GetAsSpan()
        {
            return collection.AsSpan();
        }
        public static ReadOnlySpan<T> GetAsReadonlySpan()
        {
            return new ReadOnlySpan<T>(collection);
        }
        public static IEnumerable<T> GetAsEnumerable()
        {
            return collection.AsEnumerable();
        }
        

    }
}
