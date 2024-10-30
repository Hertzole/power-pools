#nullable enable

using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enabled reusing instances of the given type.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of object to pool.</typeparam>
	public abstract class ObjectPool<T> : IDisposable where T : class, new()
	{
		/// <summary>
		///     Retrieves a shared <see cref="ObjectPool{T}" /> instance.
		/// </summary>
		public static ObjectPool<T> Shared { get; } = new SharedObjectPool<T>();

		/// <summary>
		///     Returns the amount of items that can be stored in the pool before it needs to resize.
		/// </summary>
		public abstract int Capacity { get; }

		/// <summary>
		///     Returns the amount of items that are currently in the pool.
		/// </summary>
		public abstract int InPool { get; }

		/// <summary>
		///     Returns the amount of items that have been taken from the pool and not returned.
		/// </summary>
		public virtual int InUse { get; protected set; }

		/// <summary>
		///     Retrieves an item from the pool. If the pool is empty, a new item will be created.
		/// </summary>
		/// <remarks>
		///     The item should be returned to the pool when it's no longer needed via <see cref="Return" /> so that it can be
		///     reused. It's not a fatal error to not return an item, but failure to do so may lead to decreased application
		///     performance.
		/// </remarks>
		/// <returns>A newly created item or a reused item.</returns>
		public abstract T Rent();

		/// <summary>
		///     Returns an item to the pool that was previously obtained via <see cref="Rent" />.
		/// </summary>
		/// <remarks>
		///     Once the item has been returned, it should not be used anymore. Doing so may result in unexpected behavior.
		/// </remarks>
		/// <param name="item">The item to return to the pool.</param>
		public abstract void Return(T item);

		/// <summary>
		///     Ensures that the pool has at least the specified amount of items in it.
		/// </summary>
		/// <param name="count">The amount of items to ensure are in the pool.</param>
		/// <returns>The amount of items that were created.</returns>
		public abstract int PreWarm(int count);

		/// <summary>
		///     Creates a new instance of <see cref="ObjectPool{T}" /> using the default options.
		/// </summary>
		/// <returns>A new <see cref="ObjectPool{T}" /> instance.</returns>
		public static ObjectPool<T> Create()
		{
			return new ConfigurableObjectPool<T>();
		}

		/// <summary>
		///     Creates a new instance of <see cref="ObjectPool{T}" /> with an optional action to run when an item is rented and
		///     returned.
		/// </summary>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <returns>A new <see cref="ObjectPool{T}" /> instance.</returns>
		public static ObjectPool<T> Create(Action<T>? onRent, Action<T>? onReturn)
		{
			return new ConfigurableObjectPool<T>(null, onRent, onReturn);
		}

		/// <summary>
		///     Creates a new instance of <see cref="ObjectPool{T}" /> with a factory method to create new items.
		/// </summary>
		/// <param name="factory">A factory method to create new items.</param>
		/// <returns>A new <see cref="ObjectPool{T}" /> instance.</returns>
		public static ObjectPool<T> Create(Func<T> factory)
		{
			return new ConfigurableObjectPool<T>(factory);
		}

		/// <summary>
		///     Creates a new instance of <see cref="ObjectPool{T}" /> with a factory method to create new items and optional
		///     actions to run when an item is rented and returned.
		/// </summary>
		/// <param name="factory">A factory method to create new items.</param>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <returns>A new <see cref="ObjectPool{T}" /> instance.</returns>
		public static ObjectPool<T> Create(Func<T> factory, Action<T>? onRent, Action<T>? onReturn)
		{
			return new ConfigurableObjectPool<T>(factory, onRent, onReturn);
		}

		/// <summary>
		///     Disposes the pool and all of its items.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Disposes the pool and all of its items.
		/// </summary>
		protected virtual void Dispose(bool disposing) { }
	}
}