# SnapshotIt

🎉 **SnapshotIt** is a lightweight, extensible C# library for .NET that allows you to easily snapshot (capture), restore, and copy the state of your objects at any point in your application. Perfect for undo/redo functionality, state recovery, and seamless state management!

![SnapshotIt Logo](https://github.com/AkhmedovEhson/SnapshotIt/blob/main/assets/iconforgithub.png)

---

## ✨ Features

- **Snapshotting**: Instantly capture the state of any object and restore it when needed.
- **Copying**: Deep-copy objects to avoid unwanted side effects.
- **Unbounded Channel (`BufferBlock<T>`)**: Use as an unbounded channel for posting, reading, and closing items.
- **Dependency Injection Helpers**: Easy integration with ASP.NET and .NET DI containers.
- **Extensible**: Add new features and extensions easily.
- **FluentValidation Integration**: Validate objects when saving or restoring snapshots (see [`SnapshotIt.FluentValidation`](./src/SnapshotIt.FluentValidation/README.md)).

---

## 🚦 Requirements

- .NET 6 or .NET 7

---

## 📦 Installation

Add the package to your project (coming soon via NuGet):

```bash
dotnet add package SnapshotIt
```

Or clone this repo and reference directly.

---

## 📚 Documentation & Usage

### 1. Snapshotting and Copying

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Copy class
var product = new Product { Id = 1, Name = "Product" }; // reference type, mutable
var product2 = Snapshot.Out.Copy(product); // deep copy

// Create a snapshot collection
Snapshot.Out.Create<Product>(10); // initialize collection with room for 10 snapshots

// Capture the state
Snapshot.Out.Post(product);

// Mutate the object
product.Id = 2;
product.Name = "Gucci";

// Restore previous state
var capturedProduct = Snapshot.Out.Get<Product>(0); // get first snapshot
var capturedByPredicate = Snapshot.Out.Get<Product>(p => p.Id == 1);
```

**Logging Example:**

```csharp
log.Information($"The product's name is {product.Name}"); // Logs: Gucci
log.Information($"The previous product's name was {capturedProduct.Name}"); // Logs: Nike
```

---

### 2. ASP.NET & Dependency Injection

```csharp
using SnapshotIt.DependencyInjection;

// Standard DI via constructor
public class UserController : BaseController
{
    public ILogger logger;
    public UserController(ILogger logger)
    {
        this.logger = logger;
    }
}

// Or get directly from DI container
public class UserController : BaseController
{
    public ILogger logger = Connector.GetService<ILogger>();
}
```

#### Service Registration

```csharp
using SnapshotIt.DependencyInjection;

[RuntimeDependencyInjectionOption(Lifetime = ServiceLifetime.Scoped)]
public class ProductRepository : IProductRepository {}

[RuntimeDependencyInjectionOption(Lifetime = ServiceLifetime.Transient)]
public class ColorRepository : IColorRepository {}

var runtimeServices = new RuntimeRegisterServices(Assembly.GetExecutingAssembly(), services);
runtimeServices.ConfigureAllServices();
```

---

## 🧪 FluentValidation Integration

See [`SnapshotIt.FluentValidation/README.md`](./src/SnapshotIt.FluentValidation/README.md) for details.

- Validate objects before posting or retrieving snapshots.
- Synchronous and asynchronous validation support.
- Comprehensive error handling.

Example:

```csharp
var validator = new ProductValidator();
Snapshot.Out.Post(product, validator);
var validatedProduct = Snapshot.Out.Get<Product>(0, validator);
```

---

## 🤝 Contributing

NOT ALLOWED !

---


## 🧑 Author

- [@AkhmedovEhson](https://github.com/AkhmedovEhson)

---

Happy coding! 🚀

