using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.DependencyInjection.UnitTests
{
    public class ProductTests
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ColourTests
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ColourTestsAsScoped
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
