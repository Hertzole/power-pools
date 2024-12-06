#nullable enable

using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Base class for creating collection pools.
	/// </summary>
	/// <typeparam name="TCollection">The type of collection.</typeparam>
	public abstract class CollectionPool<TCollection> : IObjectPool<TCollection> where TCollection : class
	{
		private readonly ConfigurableObjectPool<TCollection> pool;

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

		internal CollectionPool(Func<TCollection> factory, Action<TCollection> onReturnAndDispose)
		{
			pool = new ConfigurableObjectPool<TCollection>(factory, onReturn: onReturnAndDispose, onDispose: onReturnAndDispose);
		}

		/// <inheritdoc />
		public TCollection Rent()
		{
			return pool.Rent();
		}

		/// <inheritdoc />
		public void Return(TCollection item)
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
			GC.SuppressFinalize(this);
		}
	}
}