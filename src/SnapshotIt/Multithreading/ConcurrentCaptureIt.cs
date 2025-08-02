using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using SnapshotIt;
using SnapshotIt.Multithreading;


namespace SnapshotIt.Domain.Utils;

internal static partial class CaptureIt<T>
{
    private static Channel<Pocket<T>> _channel = Channel.CreateUnbounded<Pocket<T>>();
    private static ManualResetEvent _locker = new ManualResetEvent(true); // signaled ...

    public static ChannelWriter<Pocket<T>> Writer
    {
        get
        {
            bool CanWrite = _channel.Writer.WaitToWriteAsync().AsTask().GetAwaiter().GetResult();

            if (CanWrite is false)
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


    private static void Dispose()
    {
        _locker.Dispose();
    }

    /// <summary>
    /// `Clear` - clears captures ...
    /// </summary>
    public static void Clear()
    {
        try
        {
            _locker.Reset();

            if (Reader?.Count > 0)
            {
                _channel = Channel.CreateUnbounded<Pocket<T>>();
            }

            collection = null;
            size = 0;
            index = 0;

        }
        finally
        {
            _locker.Set();
            Dispose();
        }

    }

    /// <summary>
    /// `PostAsync` - posts object to collection of captures concurrently.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.None)] T[] values,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _locker.WaitOne();

            for (int i = 0; i < values.Length; i++)
            {
                Type type = typeof(T);
                var result = values[i];

                // Initializes the type
                T instance = type.IsClass
                    ? Snapshot.Out.Copy<T>(result)
                    : result;

                var currentIndex = Interlocked.Increment(ref index) - 1;
                await Writer.WriteAsync(new Pocket<T>() { Index = currentIndex, Value = instance },cancellationToken);
            }
        }
        finally
        {
            Writer.Complete();
        }
    }

    public static async Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.None)] T value,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _locker.WaitOne();

            var currentIndex = Interlocked.Increment(ref index) - 1;
            await Writer.WriteAsync(new Pocket<T>() { Index = currentIndex, Value = value },cancellationToken);
        }
        finally
        {
            Writer.Complete();
        }
    }

    /// <summary>
    /// `GetAllAsync` - gets all captures asynchronously and returns them as an array.
    /// </summary>
    /// <returns>As a response, there is an instance of <seealso cref="Array"/></returns>
    public static async Task<T[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _locker.WaitOne();
        await FillCollection(cancellationToken);
        return collection;
    }
    /// <summary>
    /// `GetAsync` - gets capture by index asynchronously.
    /// </summary>
    /// <param name="ind"></param>
    /// <returns>As a response, there is an instance of captured object</returns>
    public static async Task<T> GetAsync(int ind,CancellationToken cancellationToken = default)
    {
        _locker.WaitOne();
        await FillCollection(cancellationToken);
        return collection[ind];
    }

    /// <summary>
    /// `FillCollection` - fills the collection with captured objects from the channel asynchronously.
    /// </summary>
    /// <returns></returns>
    private static async Task FillCollection(CancellationToken cancellationToken)
    {

        if (Reader.Count > 0)
        {
            await foreach (var item in Reader.ReadAllAsync(cancellationToken))
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
}
