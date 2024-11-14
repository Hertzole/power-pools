#nullable enable

#if NET9_0_OR_GREATER
using Lock = System.Threading.Lock;
#else
using Lock = object;
#endif
using System;

namespace Hertzole.PowerPools
{
	internal class ConfigurableObjectPool<T> : ObjectPool<T> where T : class, new()
	{
		private readonly Lock lockObject = new Lock();

		private readonly PooledStack<T> pool;
		private readonly Func<T>? factory;
		private readonly Action<T>? onRent;
		private readonly Action<T>? onReturn;

		public override int Capacity
		{
			get
			{
				lock (lockObject)
				{
					return pool.Capacity;
				}
			}
		}
		public override int InPool
		{
			get
			{
				lock (lockObject)
				{
					return pool.Length;
				}
			}
		}

		public ConfigurableObjectPool(int initialCapacity = 16, Func<T>? factory = null, Action<T>? onRent = null, Action<T>? onReturn = null)
		{
			pool = new PooledStack<T>(initialCapacity);
			this.factory = factory;
			this.onRent = onRent;
			this.onReturn = onReturn;
		}

		public override T Rent()
		{
			lock (lockObject)
			{
				if (!pool.TryPop(out T? item))
				{
					item = CreateItem();
				}

				InUse++;
				onRent?.Invoke(item);
				return item;
			}
		}

		public override void Return(T item)
		{
			lock (lockObject)
			{
				InUse--;
				onReturn?.Invoke(item);
				pool.Push(item);
			}
		}

		public override int PreWarm(int count)
		{
			lock (lockObject)
			{
				// There are already enough items in the pool. No need to do anything.
				if (pool.Length >= count)
				{
					return 0;
				}

				int created = 0;
				for (int i = pool.Length; i < count; i++)
				{
					pool.Push(CreateItem());
					created++;
				}

				return created;
			}
		}

		private T CreateItem()
		{
			return factory != null ? factory() : new T();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (lockObject)
				{
					pool.Dispose();
					InUse = 0;
				}
			}
		}
	}
}