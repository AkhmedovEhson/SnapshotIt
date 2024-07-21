using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils;

public static partial class CaptureIt<T>
{

    /// <summary>
    /// `PostAsync` - posts object to collection of captures concurrently.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T value)
    {

        // WIP: Initialization of the new `Task` :)
        var task = Task.Run(() =>
        {       
            var spinLock = new SpinLock();
            bool _locked = false;

            Type type = typeof(T);

            // Initializes the type
            T instance = type.IsClass
                ? Snapshot.Out.Copy<T>(value)
                : value;

            // locks the thread.
            spinLock.Enter(ref _locked);

            int _size = collection.Length;
            if (index == (_size - 1))
            {
                var array = new T[collection.Length * 2];
                Array.Copy(collection, array, collection.Length);
                collection = array;
            }
            collection[index++] = instance;

            spinLock.Exit();

        });

        return task;
    }


    /// <summary>
    /// `PostAsync` - posts object to collection of captures concurrently.
    /// <br/> `pos` - the position of collection, helps to determine the position in collection of captures
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T value,
                                                                                 uint pos = 1)
    {
        // WIP: Initialization of the new `Task` :)
        var task = Task.Run(() =>
        {
            var spinLock = new SpinLock();
            bool _locked = false;

            Type type = typeof(T);

            // Initializes the type
            T instance = type.IsClass
                ? Snapshot.Out.Copy<T>(value)
                : value;

            // locks the thread.
            spinLock.Enter(ref _locked);

            int _size = collection.Length;

            // Checks if provided position exists in collection and checks if current pointer points to the end of collection
            if (index + pos >= _size || index == (_size - 1))
            {
                var array = new T[collection.Length * 2];
                Array.Copy(collection, array, collection.Length);
                collection = array;
            }
            collection[index + pos] = instance;

            spinLock.Exit();
        });

        return task;
    }
}
