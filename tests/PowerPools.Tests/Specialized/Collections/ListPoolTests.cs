using System;
using System.Collections.Generic;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class ListPoolTests : CollectionPoolTests<ListPool<object>, List<object>>
	{
		protected override ListPool<object> CreatePool(int capacity,
			Action<List<object>>? onRent = null,
			Action<List<object>>? onReturn = null,
			Action<List<object>>? onDispose = null)
		{
			return ListPool<object>.Create();
		}

		protected override IObjectPool<List<object>> GetShared()
		{
			return ListPool<object>.Shared;
		}

		protected override void AddToCollection(List<object> item)
		{
			for (int i = 0; i < 100; i++)
			{
				item.Add(new object());
			}
		}

		protected override void AssertCount(List<object> item, int count)
		{
			Assert.That(item.Count, Is.EqualTo(count));
		}
	}
}