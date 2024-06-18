using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Utils
{
    /// <summary>
    /// <seealso cref="CaptureIt{T}" /> provides bunch of APIs to collect data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static partial class CaptureIt<T>
    {
        private static T[] collection = new T[Constants.SizeOfCaptures];
        private static int index = 0;
        private static int size = 0;



        /// <summary>
        /// Gets captured object from captures by index, otherwise if provided index is out of range, throws <seealso cref="IndexOutOfRangeException"/>
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T Get(int ind)
        {
            if (ind < 0 || ind >= (collection.Length))
            {
                throw new IndexOutOfRangeException($"Index is out of range, input ( {typeof(T).FullName} )");
            }
 
            return collection[ind];
        }
        /// <summary>
        /// Gets captured object from captures using expressions, else throws <seealso cref="NullReferenceException"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static T Get(Func<T,bool> expression)
        {
            var query = collection.Where(expression).FirstOrDefault();

            if (query is null)
            {
                throw new NullReferenceException($"Type {typeof(T).Name} is not found in collection");
            }

            return query;
        }
        /// <summary>
        /// Creates new collection of captures with provided size
        /// </summary>
        /// <param name="s"></param>
        public static void Create(int s = 1)
        {
            collection = new T[s];
            size = s;
            index = 0;
        }

        /// <summary>
        /// Copies value and pastes in collection
        /// </summary>
        /// <param name="value"></param>
        public static void Post([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T value)
        {
            Type type = typeof(T);

            T instance = type.IsClass
                ? Snapshot.Out.Copy<T>(value)
                : value;

            if (index == collection.Length - 1)
            {
                var array = new T[collection.Length * 2];
                Array.Copy(collection,array,collection.Length);
                collection = array;
            }
            collection[index++] = instance;
        }
        /// <summary>
        /// Resets collection, makes `collection` for pointing to null
        /// </summary>
        /// <param name="s"></param>
        public static void Reset(int? s)
        {
            collection = new T[!s.HasValue ? size : s.Value];
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="Span{T}"/>
        /// </summary>
        /// <returns></returns>
        public static Span<T> GetAsSpan()
        {
            return collection.AsSpan();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="ReadOnlySpan{T}"/>
        /// </summary>
        /// <returns></returns>
        public static ReadOnlySpan<T> GetAsReadonlySpan()
        {
            return new ReadOnlySpan<T>(collection);
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="IEnumerable{T}"/>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetAsEnumerable()
        {
            return collection.AsEnumerable();
        }
        

    }
}
