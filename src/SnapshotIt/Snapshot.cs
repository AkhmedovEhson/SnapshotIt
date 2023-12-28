using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt
{
    public interface ISnapshot { }

    /// <summary>
    /// Snapshot is entry-component to all API
    /// </summary>
    public class Snapshot : ISnapshot
    {
        public static ISnapshot Out { get; set; }
    }
}
