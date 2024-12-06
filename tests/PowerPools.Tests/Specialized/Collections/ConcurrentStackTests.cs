using System;
using System.Collections.Concurrent;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class ConcurrentStackPoolTests : CollectionPoolTests<ConcurrentStackPool<object>, ConcurrentStack<object>>
	{
		protected override ConcurrentStackPool<object> CreatePool(int capacity,
			Action<ConcurrentStack<object>>? onRent = null,
			Action<ConcurrentStack<object>>? onReturn = null,
			Action<ConcurrentStack<object>>? onDispose = null)
		{
			return ConcurrentStackPool<object>.Create();
		}

		protected override IObjectPool<ConcurrentStack<object>> GetShared()
		{
			return ConcurrentStackPool<object>.Shared;
		}

		protected override void AddToCollection(ConcurrentStack<object> item)
		{
			for (int i = 0; i < 100; i++)
			{
				item.Push(new object());
			}
		}

		protected override void AssertCount(ConcurrentStack<object> item, int count)
		{
			Assert.That(item.Count, Is.EqualTo(count));
		}
	}
}