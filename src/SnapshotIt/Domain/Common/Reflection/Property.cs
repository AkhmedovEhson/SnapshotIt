using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnapshotIt.Domain.Common.Reflection
{
    public static class PropertyReflection
    {
        public static void SetProperties<T,T2>(T arg,ref T2 arg2)
        {
            var properties = arg.GetType().GetProperties();
            var properties2 = arg2.GetType().GetProperties();

            if (properties.Any())
            {
                for(int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name == properties2[i].Name)
                    {
                        properties2[i].SetValue(arg2, properties[i].GetValue(arg));
                    }
                }
            }
        }
    }
}
