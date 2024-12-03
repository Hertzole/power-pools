using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	public abstract class BaseFixedSizeObjectPoolTests<TPool, TItem> : BaseObjectPoolTests<TPool, TItem> where TPool : IObjectPool<TItem> where TItem : class
	{
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
			Assert.Throws<PoolFullException>(() => Pool.Return(Factory()));
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

		protected int GetRandomCapacity()
		{
			return Random.Next(10, 500);
		}

		protected abstract TItem Factory();
	}
}