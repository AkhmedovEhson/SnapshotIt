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

    public static class Snaps<T>
    {
        private static readonly BufferBlock<T> _buffer = new();
        public static void Push(in T entity)
        {
            _buffer.Post<T>(entity);          
        }


        public static IAsyncEnumerable<T> ReadAllAsync()
        {
            return _buffer.ReceiveAllAsync();
        }


    }


}
