#nullable enable

using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of the given type.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of object to pool.</typeparam>
	public sealed class ObjectPool<T> : IObjectPool<T> where T : class
	{
		private readonly ConfigurableObjectPool<T> pool;

		/// <inheritdoc />
		public int Capacity
		{
			get { return pool.Capacity; }
		}

		/// <inheritdoc />
		public int InPool
		{
			get { return pool.InPool; }
		}

		/// <inheritdoc />
		public int InUse
		{
			get { return pool.InUse; }
		}

		private ObjectPool(Func<T> factory, Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null, int initialCapacity = 16)
		{
			ThrowHelper.ThrowIfNull(factory, nameof(factory));

			pool = new ConfigurableObjectPool<T>(factory, onRent, onReturn, onDispose, initialCapacity);
		}

		/// <inheritdoc />
		public T Rent()
		{
			return pool.Rent();
		}

		/// <inheritdoc />
		public void Return(T item)
		{
			pool.Return(item);
		}

		/// <inheritdoc />
		public int PreWarm(int count)
		{
			return pool.PreWarm(count);
		}

		/// <summary>
		///     Creates a new instance of <see cref="ObjectPool{T}" /> with a factory method to create new items and optional
		///     actions to run when an item is rented and returned.
		/// </summary>
		/// <param name="factory">Factory method to create new items.</param>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <param name="onDispose">Optional action to run on the objects in the pool when the pool is disposed.</param>
		/// <param name="initialCapacity">
		///     The initial capacity of the pool. This will not create any items, but prepare an array
		///     with the provided size.
		/// </param>
		/// <returns>A new instance of <see cref="ObjectPool{T}" />.</returns>
		public static ObjectPool<T> Create(Func<T> factory,
			Action<T>? onRent = null,
			Action<T>? onReturn = null,
			Action<T>? onDispose = null,
			int initialCapacity = 16)
		{
			return new ObjectPool<T>(factory, onRent, onReturn, onDispose, initialCapacity);
		}

		/// <summary>
		///     Disposes the pool and all of its items.
		/// </summary>
		public void Dispose()
		{
			pool.Dispose();
		}
	}
}