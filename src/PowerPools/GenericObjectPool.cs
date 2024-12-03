#nullable enable

using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of the given type with a default constructor.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of object to pool.</typeparam>
	public sealed class GenericObjectPool<T> : IObjectPool<T> where T : class, new()
	{
		private readonly ConfigurableObjectPool<T> pool;

		/// <summary>
		///     Retrieves a shared <see cref="GenericObjectPool{T}" /> instance.
		/// </summary>
		public static GenericObjectPool<T> Shared { get; } = new GenericObjectPool<T>();

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

		private GenericObjectPool(Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null, int initialCapacity = 16)
		{
			pool = new ConfigurableObjectPool<T>(Factory, onRent, onReturn, onDispose, initialCapacity);
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

		/// <inheritdoc />
		public void Dispose()
		{
			pool.Dispose();
		}

		/// <summary>
		///     Creates a new instance of <see cref="GenericObjectPool{T}" /> with optional actions to run when an item is rented
		///     and returned.
		/// </summary>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <param name="onDispose">Optional action to run on the objects in the pool when the pool is disposed.</param>
		/// <param name="initialCapacity">
		///     The initial capacity of the pool. This will not create any items, but prepare an array
		///     with the provided size.
		/// </param>
		/// <returns>A new instance of <see cref="ObjectPool{T}" />.</returns>
		public static GenericObjectPool<T> Create(Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null, int initialCapacity = 16)
		{
			return new GenericObjectPool<T>(onRent, onReturn, onDispose, initialCapacity);
		}

		private static T Factory()
		{
			return new T();
		}
	}
}