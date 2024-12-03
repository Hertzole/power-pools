using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	public class GenericFixedSizeObjectPoolTests : BaseFixedSizeObjectPoolTests<GenericFixedSizeObjectPool<object>, object>
	{
		protected override GenericFixedSizeObjectPool<object> CreatePool(int capacity,
			Action<object>? onRent = null,
			Action<object>? onReturn = null,
			Action<object>? onDispose = null)
		{
			return GenericFixedSizeObjectPool<object>.Create(capacity, onRent, onReturn, onDispose);
		}

		protected override object Factory()
		{
			return new object();
		}
	}
}