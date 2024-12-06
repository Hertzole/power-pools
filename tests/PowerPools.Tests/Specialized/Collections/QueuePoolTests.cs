using System;
using System.Collections.Generic;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class QueuePoolTests : CollectionPoolTests<QueuePool<object>, Queue<object>>
	{
		protected override QueuePool<object> CreatePool(int capacity,
			Action<Queue<object>>? onRent = null,
			Action<Queue<object>>? onReturn = null,
			Action<Queue<object>>? onDispose = null)
		{
			return QueuePool<object>.Create();
		}

		protected override IObjectPool<Queue<object>> GetShared()
		{
			return QueuePool<object>.Shared;
		}

		protected override void AddToCollection(Queue<object> item)
		{
			for (int i = 0; i < 100; i++)
			{
				item.Enqueue(new object());
			}
		}

		protected override void AssertCount(Queue<object> item, int count)
		{
			Assert.That(item.Count, Is.EqualTo(count));
		}
	}
}