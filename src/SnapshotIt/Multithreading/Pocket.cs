using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Multithreading
{
    public class Pocket<T>
    {
        public uint Index { get; set; }
        public T Value { get; set; }
    }
}
