﻿using SnapshotIt.Domain.Common.Reflection;
using SnapshotIt.Domain.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{

    public struct Snaps<T>
    {
        public T[] captures = new T[5];
        private int index = 0;

        
        public void Push(in T entity)
        {
            T instance = (T)Activator.CreateInstance(typeof(T));

            PropertyReflection.SetProperties(entity,ref instance);

            captures[index < captures.Length - 1 ? index++ : index] = instance;
            
        }

        public SValue<T>? Get()
        {
            return new SValue<T?>() { Value = captures[index == 0 ? index : index - 1] };         
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

        public Snaps() { }

        public Snaps(T entity)
        {
            this.Push(in entity);
        }
    }


}
