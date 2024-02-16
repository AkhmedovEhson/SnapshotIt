using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.Common
{
    /// <summary>
    /// RuntimeDependencyInjectionOptionAttribute - common attribute ... 
    /// </summary>
    public class RuntimeDependencyInjectionOptionAttribute:Attribute
    {
        public ServiceLifetime Lifetime { get; set; }
    }
}
