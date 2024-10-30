using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Represents a scope in which an object is rented from an object pool. The object will be returned to the pool when
	///     the scope is disposed.
	/// </summary>
	/// <typeparam name="T">The type of object to rent.</typeparam>
	public readonly struct PoolScope<T> : IDisposable where T : class, new()
	{
		private readonly ObjectPool<T> pool;
		private readonly T item;

		internal PoolScope(ObjectPool<T> pool, T item)
		{
			this.pool = pool;
			this.item = item;
		}

		/// <summary>
		///     Returns the rented item to the pool from which the item was rented.
		/// </summary>
		public void Dispose()
		{
			pool.Return(item);
		}
	}
}