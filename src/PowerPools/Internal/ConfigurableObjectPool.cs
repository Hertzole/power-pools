#nullable enable

using System;
using System.Diagnostics;
using System.Threading;

namespace Hertzole.PowerPools
{
	internal class ConfigurableObjectPool<T> : ObjectPool<T> where T : class, new()
	{
		// Do not make this read only! It's a mutable struct.
		private SpinLock lockObject;

		private readonly PooledStack<T> pool = new PooledStack<T>(16);
		private readonly Func<T>? factory;
		private readonly Action<T>? onRent;
		private readonly Action<T>? onReturn;

		public override int Capacity
		{
			get { return pool.Capacity; }
		}
		public override int InPool
		{
			get { return pool.Length; }
		}

		public ConfigurableObjectPool(Func<T>? factory = null, Action<T>? onRent = null, Action<T>? onReturn = null)
		{
			lockObject = new SpinLock(Debugger.IsAttached);

			this.factory = factory;
			this.onRent = onRent;
			this.onReturn = onReturn;
		}

		public override T Rent()
		{
			bool lockTaken = false;
			try
			{
				lockObject.Enter(ref lockTaken);

				if (!pool.TryPop(out T? item))
				{
					item = CreateItem();
				}

				InUse++;
				onRent?.Invoke(item);
				return item;
			}
			finally
			{
				if (lockTaken)
				{
					lockObject.Exit(false);
				}
			}
		}

		public override void Return(T item)
		{
			bool lockTaken = false;
			try
			{
				lockObject.Enter(ref lockTaken);

				InUse--;
				onReturn?.Invoke(item);
				pool.Push(item);
			}
			finally
			{
				if (lockTaken)
				{
					lockObject.Exit(false);
				}
			}
		}

		public override int PreWarm(int count)
		{
			bool lockTaken = false;
			try
			{
				lockObject.Enter(ref lockTaken);

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
			finally
			{
				if (lockTaken)
				{
					lockObject.Exit(false);
				}
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
				pool.Dispose();
			}
		}
	}
}