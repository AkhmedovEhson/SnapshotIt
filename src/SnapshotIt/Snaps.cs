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

    internal static class Snaps<T>
    {
        private static BufferBlock<T> _buffer = new();

        // Note: creates default buffer-block if it is not created :)
        static Snaps()
        {
            if (_buffer is null)
            {
                CreateBufferBlock();
            }
        }

        public static void Push(in T entity) => _buffer.Post<T>(entity);
        public static IAsyncEnumerable<T> ReadAllAsync() => _buffer.ReceiveAllAsync();
        public static void CloseBuffersBlock() => _buffer.Complete();
        public static void CreateBufferBlock() => _buffer = new();
    }


}
