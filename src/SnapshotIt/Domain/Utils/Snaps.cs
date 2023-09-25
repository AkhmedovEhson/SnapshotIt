using SnapshotIt.Domain.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{

    public struct Snaps<T>
    {
        public T[] captures = new T[5]; 

        
        public void Push(in T entity)
        {
            int idx = captures.Length - 1;
            
            for(int i = 0; i < captures.Length; i++)
            {
                if (captures[i] == null)
                {
                    idx = i;
                    break;
                }
            }
            captures[idx] = entity;
        }

        public SValue<T> Get()
        {
            if (captures[0]?.GetType().Name != typeof(T).Name)
                return default(SValue<T>);

            return new SValue<T>() { Value = captures[0] }; 
        }
        public SValue<T> Get(int pos)
        {
            T captured;
            try
            {
                captured = captures[pos];
            }
            catch(IndexOutOfRangeException)
            {
                Console.WriteLine("INFO: Index is out of range, snapshots are 5. Your choosed position " + (pos - 1));
                throw;
            }

            return new SValue<T>() { Value = captured };
        }
        public Snaps()
        { }
    }


}
