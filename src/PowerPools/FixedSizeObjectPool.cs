#nullable enable

#if NET9_0_OR_GREATER
using Lock = System.Threading.Lock;
#else
using Lock = object;
#endif
using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a fixed size resource pool that enables reusing instances of the given type. The pool has a fixed capacity
	///     and will throw an exception if the capacity is reached.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T"></typeparam>
	public sealed class FixedSizeObjectPool<T> : IObjectPool<T> where T : class
	{
		private readonly Lock lockObject = new Lock();

		private readonly ConfigurableObjectPool<T> internalPool;

		private int capacity;

		/// <inheritdoc />
		public int Capacity
		{
			get
			{
				lock (lockObject)
				{
					return capacity;
				}
			}
		}

		/// <inheritdoc />
		public int InPool
		{
			get { return internalPool.InPool; }
		}

		/// <inheritdoc />
		public int InUse
		{
			get { return internalPool.InUse; }
		}

		private FixedSizeObjectPool(int capacity, Func<T> factory, Action<T>? onRent = null, Action<T>? onReturn = null, Action<T>? onDispose = null)
		{
			ThrowHelper.ThrowIfNull(factory, nameof(factory));
			
			this.capacity = capacity;
			internalPool = new ConfigurableObjectPool<T>(factory, onRent, onReturn, onDispose, capacity);
		}

		/// <inheritdoc />
		/// <exception cref="PoolExhaustedException">The amount of items in use is equal or greater than the pool's capacity.</exception>
		public T Rent()
		{
			if (InUse >= Capacity)
			{
				throw new PoolExhaustedException();
			}

			return internalPool.Rent();
		}

		/// <inheritdoc />
		/// <exception cref="PoolFullException">The amount of items in the pool is equal or greater than the pool's capacity.</exception>
		public void Return(T item)
		{
			if (InPool >= Capacity)
			{
				throw new PoolFullException();
			}

			internalPool.Return(item);
		}

		/// <inheritdoc />
		/// <exception cref="PoolFullException">The amount of items to pre-warm is greater than the pool's capacity.</exception>
		public int PreWarm(int count)
		{
			if (count > Capacity)
			{
				throw new PoolFullException("Can't pre-warm the pool with more items than the pools capacity.");
			}

			return internalPool.PreWarm(count);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			internalPool.Dispose();
			lock (lockObject)
			{
				capacity = 0;
			}
		}

		/// <summary>
		///     Creates a new instance of <see cref="FixedSizeObjectPool{T}" />.
		/// </summary>
		/// <param name="capacity">The capacity of the pool. This can not be changed after creation.</param>
		/// <param name="factory">Factory method to create new items.</param>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <param name="onDispose">Optional action to run on the objects in the pool when the pool is disposed.</param>
		/// <returns>A new <see cref="FixedSizeObjectPool{T}" /> instance.</returns>
		public static FixedSizeObjectPool<T> Create(int capacity,
			Func<T> factory,
			Action<T>? onRent = null,
			Action<T>? onReturn = null,
			Action<T>? onDispose = null)
		{
			return new FixedSizeObjectPool<T>(capacity, factory, onRent, onReturn, onDispose);
		}
	}
}