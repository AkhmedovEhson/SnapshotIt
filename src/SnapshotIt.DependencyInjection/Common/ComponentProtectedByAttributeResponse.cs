using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.Common
{
    public class ComponentProtectedByAttributeResponse
    {
        public ServiceLifetime ServiceLifetime { get; set; }
        public Type Type { get; set; } 
    }
}
