using SnapshotIt.Domain;
using SnapshotIt.Domain.Common.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace SnapshotIt
{
    /// <summary>
    /// <seealso cref="CloneExtensions"/> - clones state with all it's props
    /// </summary>
    public static class CloneExtensions
    {
        /// <summary>
        /// Copies 1'st class to another, throws <seealso cref="ArgumentNullException"/> if `input` is null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static T Copy<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]T>(
            this ISnapshot _,
            [NotNull]T? input) 
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input),"Input is null, it can not be copied");
            }
            return ActivatorIt.CreateInstanceWithCopyingProperties<T>(input);
        }


    }
}
