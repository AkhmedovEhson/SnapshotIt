using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace SnapshotIt.FluentValidation
{
    /// <summary>
    /// FluentValidation extensions for SnapshotIt capturing functionality
    /// </summary>
    public static class CaptureValidationExtensions
    {
        /// <summary>
        /// Posts an object to the snapshot collection with validation using FluentValidation
        /// </summary>
        /// <typeparam name="T">The type of object to validate and post</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="input">The object to validate and post</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <exception cref="ValidationException">Thrown when validation fails</exception>
        public static void PostWithValidation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this ISnapshot snapshot,
            T input,
            IValidator<T> validator)
        {
            var validationResult = validator.Validate(input);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            snapshot.Post(input);
        }

        /// <summary>
        /// Posts an object to the snapshot collection with validation using FluentValidation asynchronously
        /// </summary>
        /// <typeparam name="T">The type of object to validate and post</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="input">The object to validate and post</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <exception cref="ValidationException">Thrown when validation fails</exception>
        public static async Task PostWithValidationAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this ISnapshot snapshot,
            T input,
            IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(input);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await snapshot.PostAsync(input);
        }

        /// <summary>
        /// Posts multiple objects to the snapshot collection with validation using FluentValidation asynchronously
        /// </summary>
        /// <typeparam name="T">The type of objects to validate and post</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="values">The objects to validate and post</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <exception cref="ValidationException">Thrown when validation fails for any object</exception>
        public static async Task PostWithValidationAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this ISnapshot snapshot,
            T[] values,
            IValidator<T> validator)
        {
            var validationTasks = values.Select(value => validator.ValidateAsync(value));
            var validationResults = await Task.WhenAll(validationTasks);
            
            var allErrors = validationResults
                .Where(result => !result.IsValid)
                .SelectMany(result => result.Errors)
                .ToList();

            if (allErrors.Count > 0)
            {
                throw new ValidationException(allErrors);
            }

            await snapshot.PostAsync(values);
        }

        /// <summary>
        /// Validates an object using FluentValidation without posting to snapshot
        /// </summary>
        /// <typeparam name="T">The type of object to validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="input">The object to validate</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>ValidationResult containing validation outcome and errors</returns>
        public static ValidationResult Validate<T>(
            this ISnapshot snapshot,
            T input,
            IValidator<T> validator)
        {
            return validator.Validate(input);
        }

        /// <summary>
        /// Validates an object using FluentValidation without posting to snapshot asynchronously
        /// </summary>
        /// <typeparam name="T">The type of object to validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="input">The object to validate</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>ValidationResult containing validation outcome and errors</returns>
        public static Task<ValidationResult> ValidateAsync<T>(
            this ISnapshot snapshot,
            T input,
            IValidator<T> validator)
        {
            return validator.ValidateAsync(input);
        }
    }
}