using SnapshotIt.Domain.Utils;

namespace SnapshotIt
{
    /// <summary>
    /// <seealso cref="SnapshotExtensions"/> - provides bunch of APIs to communicate with `BufferBlocks`, `Streams` so on ...
    /// </summary>
    public static class SnapshotExtensions
    {
        /// <summary>
        /// Reads from `BufferBlock`, Stream asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<T> ReadFromBuffersLine<T>(this IAsyncLines _) => Snaps<T>.ReadAllAsync();
        /// <summary>
        /// Pushs data to `BufferBlock`
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="input"></param>
        public static void PostToBuffers<T>(this IAsyncLines _,T input) => Snaps<T>.Push(input);
        /// <summary>
        /// Creates new `BufferBlock`
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        public static void CreateBufferBlock<T>(this IAsyncLines _) => Snaps<T>.CreateBufferBlock();
        /// <summary>
        /// Closes `BufferBlock`, makes it completed, `BufferBlock`'ll not be able to get some data into, anymore.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        public static void CloseBufferBlock<T>(this IAsyncLines _) => Snaps<T>.CloseBuffersBlock();
    }
}
