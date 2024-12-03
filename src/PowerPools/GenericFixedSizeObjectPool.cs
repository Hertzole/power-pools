#nullable enable

using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of the given type with a default constructor. The pool has
	///     a fixed capacity and will throw an exception if the capacity is reached.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of object to pool.</typeparam>
	public sealed class GenericFixedSizeObjectPool<T> : IObjectPool<T> where T : class, new()
	{
		private readonly FixedSizeObjectPool<T> pool;

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

		private GenericFixedSizeObjectPool(int capacity, Action<T>? onRent, Action<T>? onReturn, Action<T>? onDispose)
		{
			pool = FixedSizeObjectPool<T>.Create(capacity, Factory, onRent, onReturn, onDispose);
		}

		/// <inheritdoc />
		/// <exception cref="PoolExhaustedException">The amount of items in use is equal or greater than the pool's capacity.</exception>
		public T Rent()
		{
			return pool.Rent();
		}

		/// <inheritdoc />
		/// <exception cref="PoolFullException">The amount of items in the pool is equal or greater than the pool's capacity.</exception>
		public void Return(T item)
		{
			pool.Return(item);
		}

		/// <inheritdoc />
		/// <exception cref="PoolFullException">The amount of items to pre-warm is greater than the pool's capacity.</exception>
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
		/// <param name="capacity">The capacity of the pool. This can not be changed after creation.</param>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <param name="onDispose">Optional action to run on the objects in the pool when the pool is disposed.</param>
		/// <returns>A new instance of <see cref="ObjectPool{T}" />.</returns>
		public static GenericFixedSizeObjectPool<T> Create(int capacity, Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null)
		{
			return new GenericFixedSizeObjectPool<T>(capacity, onRent, onReturn, onDispose);
		}

		private static T Factory()
		{
			return new T();
		}
	}
}