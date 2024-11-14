using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class FixedObjectPoolTests : BaseObjectPoolTests<FixedSizeObjectPool<object>, object>
	{
		private const int CAPACITY = 69;

		protected override FixedSizeObjectPool<object> CreatePool()
		{
			return FixedSizeObjectPool<object>.Create(CAPACITY);
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
			using FixedSizeObjectPool<object> pool = FixedSizeObjectPool<object>.Create(150);
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
		public void GetShared_ThrowsNotSupportedException()
		{
			// Assert
			Assert.Throws<NotSupportedException>(() =>
			{
#pragma warning disable CS0618 // Type or member is obsolete
				_ = FixedSizeObjectPool<object>.Shared;
#pragma warning restore CS0618 // Type or member is obsolete
			});
		}

		[Test]
		public void Create_WithFactory_CallsFactory()
		{
			// Arrange
			bool called = false;
			int capacity = GetRandomCapacity();
			using FixedSizeObjectPool<object> newPool = ObjectPool<object>.CreateFixedSize(capacity, Factory);

			// Act
			newPool.Rent();

			// Assert
			Assert.That(called, Is.True);
			Assert.That(newPool.Capacity, Is.EqualTo(capacity));

			object Factory()
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
			using ObjectPool<object> newPool = ObjectPool<object>.CreateFixedSize(capacity, onRent: OnRented);

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
			using ObjectPool<object> newPool = ObjectPool<object>.CreateFixedSize(capacity, onReturn: OnReturned);

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

		private int GetRandomCapacity()
		{
			return Random.Next(10, 500);
		}
	}
}