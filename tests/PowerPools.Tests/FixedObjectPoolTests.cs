using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class FixedObjectPoolTests : BaseObjectPoolTests<FixedSizeObjectPool<object>, object>
	{
		protected override FixedSizeObjectPool<object> CreatePool(int capacity, Action<object>? onRent = null, Action<object>? onReturn = null, Action<object>? onDispose = null)
		{
			return FixedSizeObjectPool<object>.Create(capacity, Factory, onRent, onReturn, onDispose);
		}

		[Test]
		public void Rent_OverCapacity_ThrowsPoolExhaustedException()
		{
			// Arrange
			for (int i = 0; i < CurrentCapacity; i++)
			{
				Pool.Rent();
			}

			// Assert
			Assert.Throws<PoolExhaustedException>(() => Pool.Rent());
		}

		[Test]
		public void Return_OverCapacity_ThrowsPoolFullException()
		{
			// Arrange
			Pool.PreWarm(CurrentCapacity);

			// Assert
			Assert.Throws<PoolFullException>(() => Pool.Return(new object()));
		}

		[Test]
		public override void PreWarm_PreWarms([Values(1, 5, 10, 100)] int amount)
		{
			// Arrange
			using FixedSizeObjectPool<object> pool = FixedSizeObjectPool<object>.Create(150, Factory);
			pool.PreWarm(amount);

			// Assert
			Assert.That(pool.InPool, Is.EqualTo(amount));
			Assert.That(pool.Capacity, Is.AtLeast(amount));
		}

		[Test]
		public void PreWarm_OverCapacity_ThrowsPoolFullException()
		{
			// Assert
			Assert.Throws<PoolFullException>(() => Pool.PreWarm(CurrentCapacity + 1));
		}

		[Test]
		public void Create_WithFactory_CallsFactory()
		{
			// Arrange
			bool called = false;
			int capacity = GetRandomCapacity();
			using FixedSizeObjectPool<object> newPool = FixedSizeObjectPool<object>.Create(capacity, LocalFactory);

			// Act
			newPool.Rent();

			// Assert
			Assert.That(called, Is.True);
			Assert.That(newPool.Capacity, Is.EqualTo(capacity));

			object LocalFactory()
			{
				called = true;
				return new object();
			}
		}

		private int GetRandomCapacity()
		{
			return Random.Next(10, 500);
		}

		private static object Factory()
		{
			return new object();
		}
	}
}