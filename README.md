# PowerPools

PowerPools provides a simple way to manage object pools in C#. It is designed to be easy to use and flexible enough to handle a wide variety of use cases. There are several built-in pools to handle common types of objects, and you can easily create your own custom pools as well.

## 🌟 Features
- 🚀 **Fast** - Uses `ArrayPool` in the background to minimize allocations!
- ✂ **Fully trimmable and AOT compatible** - No reflection or dynamic code generation!
- 🧵 **Thread-safe** - All pools are thread-safe!
- 🌐 **Wide range of .NET support** - Supports .NET STandard 2.0 and up, including .NET Core 2.0+ and .NET 5+
- ✅ **100% test coverage** - Every line of code is tested to ensure correctness and reliability!
- 📕 **Fully documented** - Every public member is documented with XML docs!

## 💨 Quick Start

```csharp
using Hertzole.PowerPools;

// Create a new pool
ObjectPool<object> myPool = ObjectPool<object>.Create(() => new object());
// Make sure to dispose your pool when you're done with it
myPool.Dispose();

// Rent an object from the pool
object myObject = myPool.Rent();
// Rent with a scope and automatically return the object when the scope ends
using (myPool.Rent(out object myObject))
{
    // Do stuff with myObject
}

// Return the object to the pool
myPool.Return(myObject);

// Pre-warm the pool with a set number of objects
myPool.PreWarm(10);

// On rent callback
ObjectPool<object>.Create(..., onRent: obj => Console.WriteLine("Rented an object!"));
// On return callback
ObjectPool<object>.Create(..., onReturn: obj => Console.WriteLine("Returned an object!"));
// On dispose callback
ObjectPool<object>.Create(..., onDispose: obj => Console.WriteLine("Disposed an object!"));

// Generic pool to avoid constructors
GenericObjectPool<object> myGenericPool = GenericObjectPool<object>.Create();
// Some constructorless pools have shared instances
object myObj = GenericObjectPool<object>.Shared.Rent();

// Fixed size pools
FixedSizeObjectPool<object> myFixedSizePool = FixedSizeObjectPool<object>.Create(10, () => new object());

// Many built-in pools
StringBuilderPool stringBuilderPool = StringBuilderPool.Shared;
ListPool<int> listPool = ListPool<int>.Shared;
HashSetPool<int> hashSetPool = HashSetPool<int>.Shared;
StackPool<int> stackPool = StackPool<int>.Shared;
QueuePool<int> queuePool = QueuePool<int>.Shared;
ConcurrentStackPool<int> concurrentStackPool = ConcurrentStackPool<int>.Shared;
ConcurrentQueuePool<int> concurrentQueuePool = ConcurrentQueuePool<int>.Shared;
```

## 📦 Installation

![NuGet Version](https://img.shields.io/nuget/v/Hertzole.PowerPools?style=flat&logo=nuget&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FHertzole.PowerPools%2F)

You can install the package via NuGet. The package supports .NET Standard 2.0 and up.

```
Install-Package Hertzole.PowerPools
dotnet add package Hertzole.PowerPools
```

## 📚 Documentation

All pools work in the same way. You create a pool (or use a shared pool if it exists), call `Rent` to get an object, and then call `Return` to return the object to the pool. When you're done with the pool, make sure to call `Dispose` to clean up any remaining objects.

All pools implement `IObjectPool<T>` and `IDisposable`. The `IObjectPool<T>` interface has the following members:

```csharp
// The amount that can be stored in the pool before it needs to resize.
int Capacity { get; }
// The amount of objects currently in the pool.
int InPool { get; }
// The amount of objects taken from the pool but not returned.
int InUse { get; }

// Rents an object from the pool.
T Rent();
// Returns an object to the pool.
void Return(T obj);
// Creates the required amount of objects and returns how many items that were created.
int PreWarm(int count);
```

### Rent scope
You can also rent an object with a scope. This will automatically return the object to the pool when the scope ends.

```csharp
using (pool.Rent(out T obj))
{
    // Do stuff with obj
}

// Also works with using statements
using PoolScope scope = pool.RentScope(out T obj);
```

### ObjectPool

| Feature | Notes |
| --- | --- |
| Fixed size | No |
| Has shared instance | No |
| Has factory | Yes, **it's required** |
| Has on rent callback | Yes, optional |
| Has on return callback | Yes, optional |
| Has on dispose callback | Yes, optional |
| Initial capacity | 16, optional, can be changed |

```csharp
ObjectPool<object>.Create(
    // Factory method
    factory: () => new object(),
    // On rent callback
    onRent: obj => Console.WriteLine("Rented an object!"),
    // On return callback
    onReturn: obj => Console.WriteLine("Returned an object!"),
    // On dispose callback
    onDispose: obj => Console.WriteLine("Disposed an object!")
    // Initial capacity
    initialCapacity: 16,
);
```

### GenericObjectPool

| Feature | Notes |
| --- | --- |
| Fixed size | No |
| Has shared instance | Yes |
| Has factory | No |
| Has on rent callback | Yes, optional |
| Has on return callback | Yes, optional |
| Has on dispose callback | Yes, optional |
| Initial capacity | 16, optional, can be changed |

```csharp
GenericObjectPool<object>.Create(
    // On rent callback
    onRent: obj => Console.WriteLine("Rented an object!"),
    // On return callback
    onReturn: obj => Console.WriteLine("Returned an object!"),
    // On dispose callback
    onDispose: obj => Console.WriteLine("Disposed an object!")
    // Initial capacity
    initialCapacity: 16,
);
```

### FixedSizeObjectPool

| Feature | Notes |
| --- | --- |
| Fixed size | Yes |
| Has shared instance | No |
| Has factory | Yes, **it's required** |
| Has on rent callback | Yes, optional |
| Has on return callback | Yes, optional |
| Has on dispose callback | Yes, optional |
| Capacity | Fixed, must be set, can't be changed |

```csharp
FixedSizeObjectPool<object>.Create(
    // The capacity of the pool
    capacity: 16,
    // Factory method
    factory: () => new object(),
    // On rent callback
    onRent: obj => Console.WriteLine("Rented an object!"),
    // On return callback
    onReturn: obj => Console.WriteLine("Returned an object!"),
    // On dispose callback
    onDispose: obj => Console.WriteLine("Disposed an object!")
);
```

### GenericFixedSizeObjectPool

| Feature | Notes |
| --- | --- |
| Fixed size | Yes |
| Has shared instance | No |
| Has factory | No |
| Has on rent callback | Yes, optional |
| Has on return callback | Yes, optional |
| Has on dispose callback | Yes, optional |
| Capacity | Fixed, must be set, can't be changed |

```csharp
GenericFixedSizeObjectPool<object>.Create(
    // The capacity of the pool
    capacity: 16,
    // On rent callback
    onRent: obj => Console.WriteLine("Rented an object!"),
    // On return callback
    onReturn: obj => Console.WriteLine("Returned an object!"),
    // On dispose callback
    onDispose: obj => Console.WriteLine("Disposed an object!")
);
```

### StringBuilderPool

| Feature | Notes |
| --- | --- |
| Fixed size | No |
| Has shared instance | Yes |
| Has factory | No |
| Has on rent callback | No |
| Has on return callback | No |
| Has on dispose callback | No |
| Default capacity | 256, optional, can be changed, affects the capacity of the StringBuilder, not the pool |

```csharp
StringBuilderPool.Create(
    // Default string builder capacity
    defaultCapacity: 256
);
```

### Collection Pools

*If you want collections that actually use pooling I would recommend checking out [jtmueller/Collections.Pooled](https://github.com/jtmueller/Collections.Pooled)*

Collection pools encompass `ListPool`, `HashSetPool`, `StackPool`, `QueuePool`, `ConcurrentStackPool`, and `ConcurrentQueuePool`. They all have the same features.

| Feature | Notes |
| --- | --- |
| Fixed size | No |
| Has shared instance | Yes |
| Has factory | No |
| Has on rent callback | No |
| Has on return callback | No |
| Has on dispose callback | No |
| Default capacity | 16, can not be changed |

```csharp
ListPool<int>.Create();
ListPool<int>.Shared;
HashSetPool<int>.Create();
HashSetPool<int>.Shared;
StackPool<int>.Create();
StackPool<int>.Shared;
QueuePool<int>.Create();
QueuePool<int>.Shared;
ConcurrentStackPool<int>.Create();
ConcurrentStackPool<int>.Shared;
ConcurrentQueuePool<int>.Create();
ConcurrentQueuePool<int>.Shared;
```

## 📃 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.