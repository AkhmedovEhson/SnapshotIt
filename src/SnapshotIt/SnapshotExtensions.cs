using SnapshotIt.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt
{
    public static class SnapshotExtensions
    {
        public static IAsyncEnumerable<T> ReadFromBuffersLine<T>(this IAsyncLines _) => Snaps<T>.ReadAllAsync();
        public static void PostToBuffers<T>(this IAsyncLines _,T input) => Snaps<T>.Push(input);
        public static void CreateBufferBlock<T>(this IAsyncLines _) => Snaps<T>.CreateBufferBlock();
        public static void CloseBufferBlock<T>(this IAsyncLines _) => Snaps<T>.CloseBuffersBlock();
    }
}
