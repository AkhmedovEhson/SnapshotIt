using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{

    public struct Snaps<T>
    {
        private T[] captures = new T[1];

        
        public void Push(in T entity)
        {         
            captures[0] = entity;
        }

        public T Get()
        {
            if (captures[0].GetType().Name != typeof(T).Name)
                return default(T);

            return captures[0];
        }

        public Snaps()
        { }
    }


}
