using SnapshotIt.Domain.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SnapshotIt
{
    /// <summary>
    /// `CaptureExtensions` - captures state and use it during runtime of application
    /// </summary>
    public static class CaptureExtensions
    {
        private static HashSet<Type> Types { get; set; } = new HashSet<Type>();

        /// <summary>
        /// Creates new collection of captures with provided size, or recreates it 
        /// </summary>
        /// <param name="s"></param>
        public static void Create<T>(this ISnapshot _, uint size)
        {
            CaptureIt<T>.Create(size);
            Types.Add(typeof(T));
        }
        /// <summary>
        /// Clears all types created with that type .
        /// </summary>
        /// <param name="_"></param>
        public static void ClearAll(this ISnapshot _)
        {
            
            if (Types.Count > 0)
            {
                foreach(var item in Types)
                {
                    var captureItOfType = typeof(CaptureIt<>).MakeGenericType(item);

                    var clearMethod = captureItOfType.GetMethod("Clear", BindingFlags.Public | BindingFlags.Static);
                    
                    ArgumentNullException.ThrowIfNull(clearMethod, "The 'Clear' method was not found in the type.");

                    clearMethod.Invoke(null, null);
                }
            }
            Types.Clear();
        }

        public static void Clear<T>(this ISnapshot _)
        {
            var @type = Types
                .Where(o => o.Name == typeof(T).Name)
                .FirstOrDefault();

            ArgumentNullException.ThrowIfNull(type,"Provided Type is not registered by `Snapshot.Out.Create!`");

            var captureItOfType = typeof(CaptureIt<>).MakeGenericType(@type);

            var clearMethod = captureItOfType.GetMethod("Clear", BindingFlags.Public | BindingFlags.Static);

            ArgumentNullException.ThrowIfNull(clearMethod, "The 'Clear' method was not found in the type.");

            clearMethod.Invoke(null, null);
        }
        /// <summary>
        /// Copies value and pastes in collection
        /// </summary>
        /// <param name="value"></param>
        public static void Post<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T>(
            this ISnapshot _,
            T input)
        {
            CaptureIt<T>.Post(input);
        }
        /// <summary>
        /// `Reset` - resets the collection of data with new collection !
        /// </summary>
        /// <param name="size"></param>
       public static void Reset(int? size) => CaptureIt<int>.Reset(size);

        /// <summary>
        /// `PostAsync` - posts bunch of captures to collection of captures concurrently.
        /// <br/>`Task.WhenAll(Any)` - paste 10 `PostAsync` to Task.WhenAll(Any), it completes concurrently
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task PostAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this ISnapshot _,
            T[] values,
            CancellationToken cancellationToken = default)
        {
            return CaptureIt<T>.PostAsync(values,cancellationToken);
        }
        /// <summary>
        /// `PostAsync` - posts an object to collection of captures asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task PostAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
          this ISnapshot _,
          T value,
          CancellationToken cancellationToken = default)
        {
            return CaptureIt<T>.PostAsync(value,cancellationToken);
        }
        /// <summary>
        /// Gets captured object from captures by index, otherwise if provided index is out of range, throws <seealso cref="IndexOutOfRangeException"/>
        /// </summary>
        /// <param name="ind"></param>
        /// <returns>Captured object typeof <seealso cref="{T}"/></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T Get<T>(this ISnapshot _, uint ind = 0)
        {
            return CaptureIt<T>.Get(ind);
        }
        /// <summary>
        /// Gets captured object from captures using expressions, else throws <seealso cref="NullReferenceException"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Captured object typeof <seealso cref="{T}"/></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static T Get<T>(this ISnapshot _,Func<T, bool> predicate)
        {
            return CaptureIt<T>.Get(predicate);
        }
        /// <summary>
        /// `GetAsync` - gets captured object by index asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="ind"></param>
        /// <returns></returns>
        public static Task<T> GetAsync<T>(this ISnapshot _,
            int ind,
            CancellationToken cancellationToken = default)
        {
            return CaptureIt<T>.GetAsync(ind,cancellationToken);
        }

        /// <summary>
        /// `GetAllAsync` - get all captures asynchronously and returns them as an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        public static Task<T[]> GetAllAsync<T>(this ISnapshot _,
            CancellationToken cancellationToken = default)
        {
            return CaptureIt<T>.GetAllAsync(cancellationToken);
        }

        /// <summary>
        /// Responds collection of captures as <seealso cref="Span{T}"/>
        /// </summary>
        /// <returns>Captures in <seealso cref="Span{T}"/></returns>
        public static Span<T> GetAsSpan<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsSpan();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="ReadOnlySpan{T}"/>
        /// </summary>
        /// <returns>Captures in <seealso cref="ReadOnlySpan{T}"/></returns>
        public static ReadOnlySpan<T> GetAsReadonlySpan<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsReadonlySpan();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="IEnumerable{T}"/>
        /// </summary>
        /// <returns>Captures in <seealso cref="IEnumerable{T}"/> iterator</returns>
        public static IEnumerable<T> GetAsEnumerable<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsEnumerable();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns><seealso cref="List{T}"/> of captures</returns>
        public static List<T> GetAsList<T>(this ISnapshot _)
        {
            return CaptureIt<T>.GetAsList();
        }
        /// <summary>
        /// Responds collection of captures as <seealso cref="List{T}"/> with size settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="size"></param>
        /// <returns><seealso cref="List{T}"/> of captures</returns>
        public static List<T> GetAsList<T>(this ISnapshot _, uint size)
        {
            return CaptureIt<T>.GetAsList(size:size);
        }

    }
}
