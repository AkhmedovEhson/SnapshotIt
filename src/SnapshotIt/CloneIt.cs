using SnapshotIt.Domain.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt
{
    /// <summary>
    /// CloneIt<T> where T is not null !
    /// </summary>
    public class CloneIt<T> where T : notnull
    {
        /// <summary>
        /// Copies 1'st component to another
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public T Copy(T input)
        {
            var instance = Activator.CreateInstance<T>();
            PropertyReflection.SetProperties(input,ref instance);
            return instance;
        }

    }
}
