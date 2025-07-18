﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using SnapshotIt;
using SnapshotIt.Multithreading;


namespace SnapshotIt.Domain.Utils;

internal static partial class CaptureIt<T>
{
    private static Channel<Pocket<T>> _channel = Channel.CreateUnbounded<Pocket<T>>();

    public static ChannelWriter<Pocket<T>> Writer
    {
        get
        {
            if (_channel.Reader.Completion.IsCompleted)
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

        for (int i = 0; i < values.Length; i++)
        {
            Type type = typeof(T);

            // Initializes the type
            T instance = values[i];

            await Writer.WriteAsync(new Pocket<T>() { Index = Interlocked.Increment(ref index), Value = instance });
        }

        Writer.Complete();
    }

    public static async Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.None)] T value)
    {
        await Writer.WriteAsync(new Pocket<T>() { Index = Interlocked.Increment(ref index), Value = value });
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
        await foreach (var item in Reader.ReadAllAsync())
        {
            if (item.Index > (collection.Length - 1))
            {
                var array = new T[collection.Length * 2];
                Array.Copy(collection, array, collection.Length);
                collection = array;
            }

            T instance = item.Value;

            collection[item.Index] = instance;
        }

    }

}
