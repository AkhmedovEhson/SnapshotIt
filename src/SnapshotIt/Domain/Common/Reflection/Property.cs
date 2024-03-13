using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SnapshotIt.Domain.Common.Reflection
{
    public static class PropertyReflection
    {
        // Note: Complexity `O(n)`
        // Please provide your decisions about it, if there is a way to improve complexity, just pull your request.
        /// <summary>
        /// Fills properties of component dynamically.
        /// <br/>Argument by reference <paramref name="arg2"/>, so all changes effact to original variable(arg2)
        /// </summary>
        public static void SetProperties<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T2>(T arg,ref T2 arg2)
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
