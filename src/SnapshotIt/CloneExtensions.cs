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
        /// Copies 1'st component to another
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Copy<T>(
            this ISnapshot _,
            T input) where T : notnull
        {
            var instance = Activator.CreateInstance<T>();
            PropertyReflection.SetProperties(input, ref instance);
            return instance;


        }
    }
}
