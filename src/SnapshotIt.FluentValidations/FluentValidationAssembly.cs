using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.FluentValidations
{
    /// <summary>
    /// Adjusts assemblies
    /// </summary>
    public static class FluentValidationAssembly
    {
        /// <summary>
        /// The `assembly` where the `ValidationContext` will be searched.
        /// </summary>
        public static Assembly? Assembly { get; private set; }
        /// <summary>
        /// Changes the current assembly
        /// </summary>
        /// <param name="assembly"></param>
        public static void ConfigureAssembly(Assembly assembly) => Assembly = assembly;
    }
}
