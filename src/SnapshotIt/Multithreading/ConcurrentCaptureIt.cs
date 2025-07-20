using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using SnapshotIt;
using SnapshotIt.Multithreading;


namespace SnapshotIt.Domain.Utils;

internal static partial class CaptureIt<T>
{
    private static Channel<Pocket<T>> _channel = Channel.CreateUnbounded<Pocket<T>>();
    private static Semaphore _semaphore = new Semaphore(1, 1);

    public static ChannelWriter<Pocket<T>> Writer
    {
        get
        {
            bool IsCompleted = _channel.Writer.WaitToWriteAsync().AsTask().GetAwaiter().GetResult();

            if (IsCompleted is false)
            {
                _channel = Channel.CreateUnbounded<Pocket<T>>();
            }
            return _channel.Writer;
        }
    }

    public static ChannelReader<Pocket<T>> Reader
    {
        get
        {
            return _channel.Reader;
        }
    }

    /// <summary>
    /// `PostAsync` - posts object to collection of captures concurrently.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.None)] T[] values)
    {
        try
        {
            _semaphore.WaitOne();
            for (int i = 0; i < values.Length; i++)
            {
                Type type = typeof(T);
                var result = values[i];

                // Initializes the type
                T instance = type.IsClass
                    ? Snapshot.Out.Copy<T>(result)
                    : result;

                await Writer.WriteAsync(new Pocket<T>() { Index = index, Value = instance });
                Interlocked.Increment(ref index);
            }
        }
        finally
        {
            Writer.Complete();
            _semaphore.Release();
        }
    }

    public static async Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.None)] T value)
    {
        try
        {
            _semaphore.WaitOne();
            await Writer.WriteAsync(new Pocket<T>() { Index = index, Value = value });
            Interlocked.Increment(ref index);

        }
        finally
        {
            Writer.Complete();
            _semaphore.Release();
        }
    }

    /// <summary>
    /// `GetAllAsync` - gets all captures asynchronously and returns them as an array.
    /// </summary>
    /// <returns>As a response, there is an instance of <seealso cref="Array"/></returns>
    public static async Task<T[]> GetAllAsync()
    {
        await FillCollection();
        return collection;
    }
    /// <summary>
    /// `GetAsync` - gets capture by index asynchronously.
    /// </summary>
    /// <param name="ind"></param>
    /// <returns>As a response, there is an instance of captured object</returns>
    public static async Task<T> GetAsync(int ind)
    {
        await FillCollection();
        return collection[ind];
    }

    /// <summary>
    /// `FillCollection` - fills the collection with captured objects from the channel asynchronously.
    /// </summary>
    /// <returns></returns>
    private static async Task FillCollection()
    {
        try
        {
            _semaphore.WaitOne();
            if (Reader.Count > 0)
            {
                await foreach (var item in Reader.ReadAllAsync())
                {
                    if (item.Index > (collection.Length - 1))
                    {
                        var array = new T[collection.Length * 2];
                        Array.Copy(collection, array, collection.Length);
                        collection = array;
                    }

                    Pocket<T> instance = item.GetType().IsClass
                        ? Snapshot.Out.Copy(item)
                        : item;

                    collection[item.Index] = instance.Value;
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

}
