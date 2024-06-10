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
    /// <br/>`Task.WhenAll(Any)` - paste 10 `PostAsync` to Task.WhenAll(Any), it completes concurrently
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


            // Note: Locking threading when touching collection ?!
            lock (locker)
            {
                collection![index == collection.Length - 1 ? index : index++] = instance;
            }

        });

        return task;
    }
}
