using System;

namespace Hertzole.PowerPools
{
	internal sealed class SharedObjectPool<T> : ConfigurableObjectPool<T> where T : class, new()
	{
		public SharedObjectPool(Func<T> factory = null, Action<T> onRent = null, Action<T> onReturn = null) : base(factory, onRent, onReturn) { }

		~SharedObjectPool()
		{
			Dispose();
		}
	}
}