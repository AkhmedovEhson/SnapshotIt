using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{
    public class CaptureIt<T>
    {
        private static T[] collection = null;
        private static int index = 0;
        private static int size = 0;

        public static T Get(int ind)
        {
            if (ind > collection.Length - 1)
            {
                throw new IndexOutOfRangeException();
            }
            return collection[ind];
        }
        public static void Create(int s)
        {
            collection = new T[s];
            size = s;
        }

        public static void Post(T value)
        {
            T instance = Activator.CreateInstance<T>();

            Type instanceType = instance.GetType();
            var props = value.GetType().GetProperties();

            for(int i = 0; i < props.Length; i++)
            {
                instanceType.GetProperty(props[i].Name)?.SetValue(instance, props[i].GetValue(value));
            }

            collection[index > collection.Length - 1 ? index : index++] = instance;
        }

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
