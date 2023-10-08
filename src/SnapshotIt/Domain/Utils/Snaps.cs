using Microsoft.EntityFrameworkCore.Infrastructure;
using SnapshotIt.Domain.Common.Reflection;
using SnapshotIt.Domain.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{

    public struct Snaps<T> where T : class, new()
    {
        public T[] captures = new T[5];
        private int index = 0;

        
        public void Push(in T entity)
        {
            T instance = new();

            PropertyReflection.SetProperties(entity,ref instance);

            captures[index < captures.Length - 1 ? index++ : index] = instance;
            
        }

        public SValue<T>? Get()
        {
            return new SValue<T?>() { Value = captures[index == 0 ? index : index - 1] };         
        }
        
        public SValue<T> Get(int pos)
        {
            if (pos >= captures.Length)
                throw new IndexOutOfRangeException();

            return new SValue<T> { Value = captures[pos] };
        }

        public Snaps() { }

        public Snaps(T entity)
        {
            this.Push(in entity);
        }
    }


}
