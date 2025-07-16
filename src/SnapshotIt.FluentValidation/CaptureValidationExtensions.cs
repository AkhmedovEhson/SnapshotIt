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
        public static async Task PostAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
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

        /// <summary>
        /// Creates new collection of captures with provided size, or recreates it with validation support
        /// </summary>
        /// <typeparam name="T">The type of objects that will be stored in the collection</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="size">The size of the collection</param>
        /// <param name="validator">The FluentValidation validator for future validations</param>
        public static void Create<T>(
            this ISnapshot snapshot,
            uint size,
            IValidator<T> validator)
        {
            snapshot.Create<T>(size);
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
                throw new ValidationException(validationResult.Errors);
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
                throw new ValidationException(validationResult.Errors);
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
                throw new ValidationException(validationResult.Errors);
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
            
            var allErrors = validationResults
                .Where(result => !result.IsValid)
                .SelectMany(result => result.Errors)
                .ToList();

            if (allErrors.Count > 0)
            {
                throw new ValidationException(allErrors);
            }

            return results;
        }

        /// <summary>
        /// Gets collection of captures as Span with validation for each object
        /// </summary>
        /// <typeparam name="T">The type of objects to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>Span of validated captured objects</returns>
        /// <exception cref="ValidationException">Thrown when any object fails validation</exception>
        public static Span<T> GetAsSpan<T>(
            this ISnapshot snapshot,
            IValidator<T> validator)
        {
            var results = snapshot.GetAsSpan<T>();
            
            foreach (var item in results)
            {
                if (item != null)
                {
                    var validationResult = validator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets collection of captures as ReadOnlySpan with validation for each object
        /// </summary>
        /// <typeparam name="T">The type of objects to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>ReadOnlySpan of validated captured objects</returns>
        /// <exception cref="ValidationException">Thrown when any object fails validation</exception>
        public static ReadOnlySpan<T> GetAsReadonlySpan<T>(
            this ISnapshot snapshot,
            IValidator<T> validator)
        {
            var results = snapshot.GetAsReadonlySpan<T>();
            
            foreach (var item in results)
            {
                if (item != null)
                {
                    var validationResult = validator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets collection of captures as IEnumerable with validation for each object
        /// </summary>
        /// <typeparam name="T">The type of objects to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>IEnumerable of validated captured objects</returns>
        /// <exception cref="ValidationException">Thrown when any object fails validation</exception>
        public static IEnumerable<T> GetAsEnumerable<T>(
            this ISnapshot snapshot,
            IValidator<T> validator)
        {
            var results = snapshot.GetAsEnumerable<T>();
            
            foreach (var item in results)
            {
                if (item != null)
                {
                    var validationResult = validator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }
                yield return item;
            }
        }

        /// <summary>
        /// Gets collection of captures as List with validation for each object
        /// </summary>
        /// <typeparam name="T">The type of objects to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>List of validated captured objects</returns>
        /// <exception cref="ValidationException">Thrown when any object fails validation</exception>
        public static List<T> GetAsList<T>(
            this ISnapshot snapshot,
            IValidator<T> validator)
        {
            var results = snapshot.GetAsList<T>();
            
            foreach (var item in results)
            {
                if (item != null)
                {
                    var validationResult = validator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets collection of captures as List with size settings and validation for each object
        /// </summary>
        /// <typeparam name="T">The type of objects to get and validate</typeparam>
        /// <param name="snapshot">The snapshot instance</param>
        /// <param name="size">The size of the list</param>
        /// <param name="validator">The FluentValidation validator to use</param>
        /// <returns>List of validated captured objects</returns>
        /// <exception cref="ValidationException">Thrown when any object fails validation</exception>
        public static List<T> GetAsList<T>(
            this ISnapshot snapshot,
            uint size,
            IValidator<T> validator)
        {
            var results = snapshot.GetAsList<T>(size);
            
            foreach (var item in results)
            {
                if (item != null)
                {
                    var validationResult = validator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }
            }

            return results;
        }
    }
}