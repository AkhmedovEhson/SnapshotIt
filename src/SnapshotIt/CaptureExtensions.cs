﻿using SnapshotIt.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt
{
    /// <summary>
    /// `CaptureExtensions` - captures state and use it during runtime of application
    /// </summary>
    public static class CaptureExtensions
    {
        /// <summary>
        /// Creates new collection of captures with provided size
        /// </summary>
        /// <param name="s"></param>
        public static void Create<T>(this ISnapshot _, int size) => CaptureIt<T>.Create(size);
        /// <summary>
        /// Copies value and pastes in collection
        /// </summary>
        /// <param name="value"></param>
        public static void Post<T>(
            this ISnapshot _,
            T input)
        {
            CaptureIt<T>.Post(input);
        }
        /// <summary>
        /// Gets data from captures by index, otherwise if provided index is out of range, throws <seealso cref="IndexOutOfRangeException"/>
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T Get<T>(this ISnapshot _,int ind)
        {
            return CaptureIt<T>.Get(ind);
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="Span{T}"/>
        /// </summary>
        /// <returns></returns>
        public static Span<T> GetAsSpan<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsSpan();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="ReadOnlySpan{T}"/>
        /// </summary>
        /// <returns></returns>
        public static ReadOnlySpan<T> GetAsReadonlySpan<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsReadonlySpan();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="IEnumerable{T}"/>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetAsEnumerable<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsEnumerable();
        }

    }
}
