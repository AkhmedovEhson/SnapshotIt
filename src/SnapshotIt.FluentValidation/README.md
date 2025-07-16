# SnapshotIt.FluentValidation

FluentValidation integration for SnapshotIt library, providing validation capabilities for capturing objects to snapshots.

## Features

- Validate objects before posting to snapshots using FluentValidation
- Support for both synchronous and asynchronous validation
- Validation of single objects and arrays
- Comprehensive error handling with ValidationException

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