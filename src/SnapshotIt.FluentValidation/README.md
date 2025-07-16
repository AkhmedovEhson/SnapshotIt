# SnapshotIt.FluentValidation

FluentValidation integration for SnapshotIt library, providing comprehensive validation capabilities for capturing and retrieving objects to/from snapshots.

## Features

- **Post Validation**: Validate objects before posting to snapshots using FluentValidation
- **Get Validation**: Validate objects when retrieving from snapshots  
- **Collection Validation**: Validate collections and arrays of objects
- **Comprehensive Coverage**: Validation overloads for all CaptureIt methods
- **Support for both synchronous and asynchronous validation**
- **Comprehensive error handling with ValidationException**

## Available Methods

### Post Methods (with validation before storing)
- `PostWithValidation<T>(T input, IValidator<T> validator)` - Synchronous validation before posting
- `PostWithValidationAsync<T>(T input, IValidator<T> validator)` - Asynchronous validation before posting
- `PostWithValidationAsync<T>(T[] values, IValidator<T> validator)` - Asynchronous validation before posting arrays

### Get Methods (with validation when retrieving)
- `GetWithValidation<T>(IValidator<T> validator, uint ind = 0)` - Get by index with validation
- `GetWithValidation<T>(Func<T, bool> predicate, IValidator<T> validator)` - Get by predicate with validation
- `GetWithValidationAsync<T>(int ind, IValidator<T> validator)` - Async get by index with validation
- `GetAllWithValidationAsync<T>(IValidator<T> validator)` - Get all items with validation

### Collection Methods (with validation when retrieving)
- `GetAsListWithValidation<T>(IValidator<T> validator)` - Get as List with validation
- `GetAsListWithValidation<T>(uint size, IValidator<T> validator)` - Get as List with size and validation
- `GetAsEnumerableWithValidation<T>(IValidator<T> validator)` - Get as IEnumerable with validation
- `GetAsSpanWithValidation<T>(IValidator<T> validator)` - Get as Span with validation
- `GetAsReadonlySpanWithValidation<T>(IValidator<T> validator)` - Get as ReadOnlySpan with validation

### Pure Validation Methods (validation without posting/retrieving)
- `Validate<T>(T input, IValidator<T> validator)` - Synchronous validation only
- `ValidateAsync<T>(T input, IValidator<T> validator)` - Asynchronous validation only

### Utility Methods
- `CreateWithValidation<T>(uint size, IValidator<T> validator)` - Create collection with validation support

## Usage

### Basic Validation

```csharp
using SnapshotIt.FluentValidation;
using FluentValidation;

// Create a validator
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

// Use with SnapshotIt
var validator = new ProductValidator();
var product = new Product { Id = 1, Name = "Test Product", Price = 99.99m };

// Post with validation
Snapshot.Out.PostWithValidation(product, validator);

// Or async
await Snapshot.Out.PostWithValidationAsync(product, validator);
```

### Validation on Retrieval

```csharp
// Get object with validation
var validatedProduct = Snapshot.Out.GetWithValidation(validator, 0);

// Get collection with validation  
var validatedList = Snapshot.Out.GetAsListWithValidation(validator);

// Get all items with validation
var validatedItems = await Snapshot.Out.GetAllWithValidationAsync(validator);
```

### Validation Without Posting

```csharp
// Just validate without posting
var result = Snapshot.Out.Validate(product, validator);
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}

// Or async
var result = await Snapshot.Out.ValidateAsync(product, validator);
```

### Array Validation

```csharp
var products = new[] { product1, product2, product3 };
await Snapshot.Out.PostWithValidationAsync(products, validator);
```

## Exception Handling

When validation fails, a `ValidationException` is thrown containing all validation errors:

```csharp
try
{
    Snapshot.Out.PostWithValidation(invalidProduct, validator);
}
catch (ValidationException ex)
{
    foreach (var error in ex.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
```

## Requirements

- .NET 8.0 or higher
- FluentValidation 11.9.0 or higher
- SnapshotIt library