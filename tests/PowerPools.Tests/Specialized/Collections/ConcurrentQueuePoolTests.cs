using System;
using System.Collections.Concurrent;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class ConcurrentQueuePoolTests : CollectionPoolTests<ConcurrentQueuePool<object>, ConcurrentQueue<object>>
	{
		protected override ConcurrentQueuePool<object> CreatePool(int capacity,
			Action<ConcurrentQueue<object>>? onRent = null,
			Action<ConcurrentQueue<object>>? onReturn = null,
			Action<ConcurrentQueue<object>>? onDispose = null)
		{
			return ConcurrentQueuePool<object>.Create();
		}

		protected override IObjectPool<ConcurrentQueue<object>> GetShared()
		{
			return ConcurrentQueuePool<object>.Shared;
		}

		protected override void AddToCollection(ConcurrentQueue<object> item)
		{
			for (int i = 0; i < 100; i++)
			{
				item.Enqueue(new object());
			}
		}

		protected override void AssertCount(ConcurrentQueue<object> item, int count)
		{
			Assert.That(item.Count, Is.EqualTo(count));
		}
	}
}