#nullable enable

#if NET9_0_OR_GREATER
using Lock = System.Threading.Lock;
#else
using Lock = object;
#endif
using System;
using System.ComponentModel;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a fixed size resource pool that enables reusing instances of the given type. The pool has a fixed capacity
	///     and will throw an exception if the capacity is reached.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T"></typeparam>
	public sealed class FixedSizeObjectPool<T> : ObjectPool<T> where T : class, new()
	{
		private readonly Lock lockObject = new Lock();

		private readonly ConfigurableObjectPool<T> internalPool;

		private int capacity;

		/// <inheritdoc />
		public override int Capacity
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
		public override int InPool
		{
			get { return internalPool.InPool; }
		}

		/// <inheritdoc />
		public override int InUse
		{
			get { return internalPool.InUse; }
		}

		/// <summary>
		///     This is not supported on <see cref="FixedSizeObjectPool{T}" />. This will always throw an
		///     <see cref="NotSupportedException" />.
		/// </summary>
		/// <exception cref="NotSupportedException"></exception>
		[Obsolete("This is not supported on FixedSizeObjectPool.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new static FixedSizeObjectPool<T> Shared
		{
			get { throw new NotSupportedException("FixedSizeObjectPool does not support a shared instance."); }
		}

		internal FixedSizeObjectPool(int capacity, Func<T>? factory = null, Action<T>? onRent = null, Action<T>? onReturn = null)
		{
			this.capacity = capacity;
			internalPool = new ConfigurableObjectPool<T>(capacity, factory, onRent, onReturn);
		}

		/// <inheritdoc />
		/// <exception cref="PoolExhaustedException">The amount of items in use is equal or greater than the pool's capacity.</exception>
		public override T Rent()
		{
			if (InUse >= Capacity)
			{
				throw new PoolExhaustedException();
			}

			return internalPool.Rent();
		}

		/// <inheritdoc />
		/// <exception cref="PoolFullException">The amount of items in the pool is equal or greater than the pool's capacity.</exception>
		public override void Return(T item)
		{
			if (InPool >= Capacity)
			{
				throw new PoolFullException();
			}

			internalPool.Return(item);
		}

		/// <inheritdoc />
		/// <exception cref="PoolFullException">The amount of items to pre-warm is greater than the pool's capacity.</exception>
		public override int PreWarm(int count)
		{
			if (count > Capacity)
			{
				throw new PoolFullException("Can't pre-warm the pool with more items than the pools capacity.");
			}

			return internalPool.PreWarm(count);
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				internalPool.Dispose();
				lock (lockObject)
				{
					capacity = 0;
				}
			}
		}

		/// <summary>
		///     Creates a new instance of <see cref="FixedSizeObjectPool{T}" />.
		/// </summary>
		/// <param name="capacity">The capacity of the pool.</param>
		/// <param name="factory">Optional factory method to create new items.</param>
		/// <param name="onRent">Optional action to run when an item is rented.</param>
		/// <param name="onReturn">Optional action to run when an item is returned.</param>
		/// <returns>A new <see cref="FixedSizeObjectPool{T}" /> instance.</returns>
		public static FixedSizeObjectPool<T> Create(int capacity, Func<T>? factory = null, Action<T>? onRent = null, Action<T>? onReturn = null)
		{
			return new FixedSizeObjectPool<T>(capacity, factory, onRent, onReturn);
		}
	}
}