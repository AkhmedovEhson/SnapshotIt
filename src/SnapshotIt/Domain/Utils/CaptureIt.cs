using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{
    public struct CaptureIt<T>
    {
        private T[] collection = null;
        private int index = 0;

        public CaptureIt(int size)
        {
            collection = new T[size];
        }

        public T this[int ind]
        {
            get
            {
                if (ind > collection.Length - 1)
                    throw new IndexOutOfRangeException();

                return collection[ind];
            }
        }

        public void Post(T value)
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

        
    }
}
