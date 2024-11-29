#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of the given type.
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
		///     Creates a new instance of <see cref="ObjectPool{T}" /> with a factory method to create new items and optional
		///     actions to run when an item is rented and returned.
		/// </summary>
		/// <param name="factory">Optional factory method to create new items.</param>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <param name="onDispose">Optional action to run on the objects in the pool when the pool is disposed.</param>
		/// <returns>A new <see cref="ObjectPool{T}" /> instance.</returns>
		public static ObjectPool<T> Create(Func<T>? factory = null, Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null)
		{
			return new ConfigurableObjectPool<T>(16, factory, onRent, onReturn, onDispose);
		}

		/// <inheritdoc cref="FixedSizeObjectPool{T}.Create(int, Func{T}?, Action{T}?, Action{T}?, Action{T}?)" />
		public static FixedSizeObjectPool<T> CreateFixedSize(int capacity, Func<T>? factory = null, Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null)
		{
			return FixedSizeObjectPool<T>.Create(capacity, factory, onRent, onReturn, onDispose);
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
		[ExcludeFromCodeCoverage] // This method for whatever reason does not want to be included in code coverage, even though it's called in Dispose.
		protected virtual void Dispose(bool disposing) { }
	}
}