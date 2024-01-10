using SnapshotIt.Domain.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SnapshotIt
{
    /// <summary>
    /// <seealso cref="CloneExtensions"/> - clones state with all it's props
    /// </summary>
    public static class CloneExtensions
    {
        /// <summary>
        /// Copies 1'st (ReferenceType) component to another, throws <seealso cref="ArgumentNullException"/> if `input` is null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static T Copy<T>(
            this ISnapshot _,
            T? input) 
        {
            if (input is null)
            {
                throw new ArgumentNullException("Input is null, null can not be copied", nameof(input));
            }
            var instance = Activator.CreateInstance<T>();
            PropertyReflection.SetProperties(input, ref instance);
            return instance;
        }


    }
}
