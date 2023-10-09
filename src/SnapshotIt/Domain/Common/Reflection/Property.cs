using System.Reflection;

namespace SnapshotIt.Domain.Common.Reflection
{
    public static class PropertyReflection
    {
        public static void SetProperties<T,T2>(T arg,ref T2 arg2)
        {
            var properties = arg.GetType().GetProperties();
            var p = arg2.GetType();

            if (properties.Any())
            {
                for(int i = 0; i < properties.Length; i++)
                {
                    p.GetProperty(properties[i].Name)!.SetValue(arg2, properties[i].GetValue(arg));
                }
            }
        }

    }
}
