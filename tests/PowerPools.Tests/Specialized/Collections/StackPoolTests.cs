using System;
using System.Collections.Generic;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class StackPoolTests : CollectionPoolTests<StackPool<object>, Stack<object>>
	{
		protected override StackPool<object> CreatePool(int capacity,
			Action<Stack<object>>? onRent = null,
			Action<Stack<object>>? onReturn = null,
			Action<Stack<object>>? onDispose = null)
		{
			return StackPool<object>.Create();
		}

		protected override IObjectPool<Stack<object>> GetShared()
		{
			return StackPool<object>.Shared;
		}

		protected override void AddToCollection(Stack<object> item)
		{
			for (int i = 0; i < 100; i++)
			{
				item.Push(new object());
			}
		}

		protected override void AssertCount(Stack<object> item, int count)
		{
			Assert.That(item.Count, Is.EqualTo(count));
		}
	}
}