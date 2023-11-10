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
using System.Threading.Tasks.Dataflow;

namespace SnapshotIt.Domain.Utils
{

    public struct Snaps<T> where T : class, new()
    {
        private readonly BufferBlock<T> _buffer = new();
        public void Push(in T entity)
        {
            _buffer.Post<T>(entity);
        }

        public SValue<T>? Get()
        {
            var item = _buffer.Receive<T>();
            return new SValue<T> { Value = item };
        }
        
        public async Task<SValue<T>> GetAsync(int pos)
        {
            T instance = null;

            if (pos >= _buffer.Count)
                throw new IndexOutOfRangeException();

            for (int position = 0; position <= pos; position++) instance = await _buffer.ReceiveAsync<T>();

            return new SValue<T>()
            {
                Value = instance
            };
        }

        public Snaps() { }

        public Snaps(T entity)
        {
            this.Push(in entity);
        }
    }


}
