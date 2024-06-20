
namespace SnapshotIt
{
    public interface ISnapshot { }
    public interface IAsyncLines { }

    /// <summary>
    /// Snapshot is entry-component to all API
    /// </summary>
    public class Snapshot : ISnapshot
    {
        public static ISnapshot Out { get; } = null!;
        public static IAsyncLines AsyncOut { get; } = null!;
    }
}
