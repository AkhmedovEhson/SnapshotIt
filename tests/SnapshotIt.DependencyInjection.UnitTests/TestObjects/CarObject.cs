
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.UnitTests.TestObjects
{
    public class Car:IRuntimeDependencyInjectionObject
    {
        public int Id { get; set; } 
    }
    public interface ICar { }
}
