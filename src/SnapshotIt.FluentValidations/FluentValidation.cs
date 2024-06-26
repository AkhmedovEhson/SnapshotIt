using SnapshotIt.FluentValidations;
using FluentValidation;
using System.Reflection;
using System;

namespace SnapshotIt.FluentValidations
{
    public static class FluentValidationAPIs
    {
        // TODO: It is required to refactor all stuff here ....
        public static void ValidateAndPost<T>(this ISnapshot _,T obj)
        {

            Assembly assembly = FluentValidationAssembly.Assembly 
                ?? Assembly.GetExecutingAssembly();

            Type? validatorType = AssemblyScanner.FindValidatorsInAssembly(assembly)
                .Select(o => o.ValidatorType)
                .Where(o => o.IsSubclassOf(typeof(AbstractValidator<T>)))
                .FirstOrDefault();

            IValidator? validator = (IValidator?)Activator.CreateInstance(validatorType);
            var context = new ValidationContext<T>(obj);
            var response = validator!.Validate(context);

            if (!response.IsValid)
            {
                throw new ArgumentException($"Input '{obj.GetType().FullName}' is not valid");
            }

            Snapshot.Out.Post<T>(obj);
        }
    }
}
