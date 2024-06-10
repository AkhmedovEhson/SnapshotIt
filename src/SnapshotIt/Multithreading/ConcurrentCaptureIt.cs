using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils;

public static partial class CaptureIt<T>
{
    private static object locker = new object();

    /// <summary>
    /// `PostAsync` - posts object to collection of captures concurrently.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task PostAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T value)
    {
        var task = Task.Run(() =>
        {
            Type type = typeof(T);

            T instance = type.IsClass
                ? Snapshot.Out.Copy<T>(value)
                : value;

            lock (locker)
            {
                collection![index == collection.Length - 1 ? index : index] = instance;
            }

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
                                                                                 int pos = 1)
    {
        var task = Task.Run(() =>
        {
            Type type = typeof(T);

            T instance = type.IsClass
                ? Snapshot.Out.Copy<T>(value)
                : value;

            // Note: Locking threading when touching collection ?!
            lock (locker)
            {
                collection![index == collection.Length - 1 ? index : index + pos] = instance;
            }

        });

        return task;
    }
}
