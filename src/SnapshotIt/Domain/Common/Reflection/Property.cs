using System.Diagnostics.CodeAnalysis;


namespace SnapshotIt.Domain.Common.Reflection
{
    public static class PropertyReflection
    {
        /// <summary>
        /// Fills properties of component dynamically.
        /// </summary>
        public static void SetProperties<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]T2>(T arg,ref T2 arg2)
        {
            var properties = arg!.GetType().GetProperties();
            var copyofInstance = arg2!.GetType();

            if (properties.Any())
            {
                for(int i = 0; i < properties.Length; i++)
                {
                    copyofInstance.GetProperty(properties[i].Name)!.SetValue(arg2, properties[i].GetValue(arg));
                }
            }
        }

    }
}
