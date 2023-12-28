using SnapshotIt.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt
{
    public static class CaptureExtensions
    {

        public static void Create<T>(this ISnapshot _, int size) => CaptureIt<T>.Create(size);

        public static void Post<T>(
            this ISnapshot _,
            T input)
        {
            CaptureIt<T>.Post(input);
        }

        public static T Get<T>(this ISnapshot _,int ind)
        {
            return CaptureIt<T>.Get(ind);
        }
        public static Span<T> GetAsSpan<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsSpan();
        }
        public static ReadOnlySpan<T> GetAsReadonlySpan<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsReadonlySpan();
        }
        public static IEnumerable<T> GetAsEnumerable<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsEnumerable();
        }

    }
}
