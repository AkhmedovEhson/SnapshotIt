using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace SnapshotIt.FluentValidation
{
    /// <summary>
    /// FluentValidation extensions for SnapshotIt capturing functionality
    /// </summary>
    public static class CaptureItValidationExtensions
    {
        /// <summary>
        /// Posts an object to the snapshot collection with validation using FluentValidation
        /// </summary>
        /// <typeparam name="T">The type of object to validate and post</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="input">The object to validate and post</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <exception cref="ValidationException">Thrown when validation fails</exception>
        public static void Post<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
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
        public static async Task PostAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this ISnapshot snapshot,
            T input,
            IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(input);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
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
        public static async Task PostAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this ISnapshot snapshot,
            T[] values,
            IValidator<T> validator)
        {
            var validationTasks = values.Select(value => validator.ValidateAsync(value));
            var validationResults = await Task.WhenAll(validationTasks);
            
            var error = validationResults
                .Where(result => !result.IsValid)
                .Select(result => result.Errors[0])
                .FirstOrDefault();

            if (error is not null)
            {
                throw new ValidationException(error.ErrorMessage);
            }

            await snapshot.PostAsync(values);
        }

        /// <summary>
        /// Gets captured object from captures by index with validation
        /// </summary>
        /// <typeparam name="T">The type of object to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="ind">The index of the object to retrieve</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>The validated captured object</returns>
        /// <exception cref="ValidationException">Thrown when the retrieved object fails validation</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when index is out of range</exception>
        public static T Get<T>(
            this ISnapshot snapshot,
            uint ind,
            IValidator<T> validator)
        {
            var result = snapshot.Get<T>(ind);
            var validationResult = validator.Validate(result);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            return result;
        }

        /// <summary>
        /// Gets captured object from captures using expressions with validation
        /// </summary>
        /// <typeparam name="T">The type of object to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="predicate">The predicate to find the object</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>The validated captured object</returns>
        /// <exception cref="ValidationException">Thrown when the retrieved object fails validation</exception>
        /// <exception cref="NullReferenceException">Thrown when object is not found</exception>
        public static T Get<T>(
            this ISnapshot snapshot,
            Func<T, bool> predicate,
            IValidator<T> validator)
        {
            var result = snapshot.Get<T>(predicate);
            var validationResult = validator.Validate(result);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            return result;
        }

        /// <summary>
        /// Gets captured object by index asynchronously with validation
        /// </summary>
        /// <typeparam name="T">The type of object to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="ind">The index of the object to retrieve</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>The validated captured object</returns>
        /// <exception cref="ValidationException">Thrown when the retrieved object fails validation</exception>
        public static async Task<T> GetAsync<T>(
            this ISnapshot snapshot,
            int ind,
            IValidator<T> validator)
        {
            var result = await snapshot.GetAsync<T>(ind);
            var validationResult = await validator.ValidateAsync(result);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            return result;
        }

        /// <summary>
        /// Gets all captures asynchronously with validation for each object
        /// </summary>
        /// <typeparam name="T">The type of objects to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>Array of validated captured objects</returns>
        /// <exception cref="ValidationException">Thrown when any retrieved object fails validation</exception>
        public static async Task<T[]> GetAllAsync<T>(
            this ISnapshot snapshot,
            IValidator<T> validator)
        {
            var results = await snapshot.GetAllAsync<T>();
            var validationTasks = results.Where(r => r != null).Select(result => validator.ValidateAsync(result));
            var validationResults = await Task.WhenAll(validationTasks);
            
            var error = validationResults
                .Where(result => !result.IsValid)
                .Select(result => result.Errors[0])
                .FirstOrDefault();

            if (error is not null)
            {
                throw new ValidationException(error.ErrorMessage);
            }

            return results;
        }


    }
}