using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.UnitTests.TestObjects
{
    public class SimpleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class o
    {
        public int id { get; set; } 
    }


    public class Obj
    {
        public Obj2 Inner { get; set; }
    }

    public class Obj2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public o O { get; set; }
    }
}
