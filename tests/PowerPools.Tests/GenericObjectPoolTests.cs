using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class GenericObjectPoolTests : BaseObjectPoolWithSharedTests<GenericObjectPool<object>, object>
	{
		protected override GenericObjectPool<object> CreatePool(int capacity,
			Action<object>? onRent = null,
			Action<object>? onReturn = null,
			Action<object>? onDispose = null)
		{
			return GenericObjectPool<object>.Create(onRent, onReturn, onDispose, capacity);
		}

		protected override IObjectPool<object> GetShared()
		{
			return GenericObjectPool<object>.Shared;
		}
	}
}