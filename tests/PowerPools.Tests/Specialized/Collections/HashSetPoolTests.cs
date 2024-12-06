using System;
using System.Collections.Generic;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class HashSetPoolTests : CollectionPoolTests<HashSetPool<object>, HashSet<object>>
	{
		protected override HashSetPool<object> CreatePool(int capacity,
			Action<HashSet<object>>? onRent = null,
			Action<HashSet<object>>? onReturn = null,
			Action<HashSet<object>>? onDispose = null)
		{
			return HashSetPool<object>.Create();
		}

		protected override IObjectPool<HashSet<object>> GetShared()
		{
			return HashSetPool<object>.Shared;
		}

		protected override void AddToCollection(HashSet<object> item)
		{
			for (int i = 0; i < 100; i++)
			{
				item.Add(new object());
			}
		}

		protected override void AssertCount(HashSet<object> item, int count)
		{
			Assert.That(item.Count, Is.EqualTo(count));
		}
	}
}