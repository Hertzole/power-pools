using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class FixedObjectPoolTests : BaseObjectPoolTests<FixedSizeObjectPool<object>, object>
	{
		private const int CAPACITY = 69;

		protected override FixedSizeObjectPool<object> CreatePool()
		{
			return FixedSizeObjectPool<object>.Create(CAPACITY, Factory);
		}

		[Test]
		public void Rent_OverCapacity_ThrowsPoolExhaustedException()
		{
			// Arrange
			for (int i = 0; i < CAPACITY; i++)
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
			Pool.PreWarm(CAPACITY);

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
			Assert.Throws<PoolFullException>(() => Pool.PreWarm(CAPACITY + 1));
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

		[Test]
		public void Create_WithRentCallback_CallsCallback()
		{
			// Arrange
			bool called = false;
			int capacity = GetRandomCapacity();
			using FixedSizeObjectPool<object> newPool = FixedSizeObjectPool<object>.Create(capacity, Factory, OnRented);

			// Act
			newPool.Rent();

			// Assert
			Assert.That(called, Is.True);
			Assert.That(newPool.Capacity, Is.EqualTo(capacity));

			void OnRented(object obj)
			{
				called = true;
			}
		}

		[Test]
		public void Create_WithReturnCallback_CallsCallback()
		{
			// Arrange
			bool called = false;
			int capacity = GetRandomCapacity();
			using FixedSizeObjectPool<object> newPool = FixedSizeObjectPool<object>.Create(capacity, Factory, onReturn: OnReturned);

			// Act
			object item = newPool.Rent();
			newPool.Return(item);

			// Assert
			Assert.That(called, Is.True);
			Assert.That(newPool.Capacity, Is.EqualTo(capacity));

			void OnReturned(object obj)
			{
				called = true;
			}
		}

		[Test]
		public void Dispose_WithDisposeCallback_CallsCallback()
		{
			// Arrange
			bool called = false;
			int capacity = GetRandomCapacity();
			FixedSizeObjectPool<object> newPool = FixedSizeObjectPool<object>.Create(capacity, Factory, onDispose: OnDisposed);
			newPool.PreWarm(capacity);

			// Act
			newPool.Dispose();

			// Assert
			Assert.That(called, Is.True);

			void OnDisposed(object obj)
			{
				called = true;
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