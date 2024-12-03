using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class ObjectPoolTests : BaseObjectPoolTests<ObjectPool<object>, object>
	{
		protected override ObjectPool<object> CreatePool(int capacity)
		{
			return ObjectPool<object>.Create(Factory, initialCapacity: capacity);
		}

		[Test]
		public void Create_WithNullFactory_ThrowsArgumentNullException()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => ObjectPool<object>.Create(null!));
		}

		[Test]
		public void Create_WithFactory_CallsFactory()
		{
			// Arrange
			bool called = false;
			using ObjectPool<object> newPool = ObjectPool<object>.Create(LocalFactory);

			// Act
			newPool.Rent();

			// Assert
			Assert.That(called, Is.True);

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
			using ObjectPool<object> newPool = ObjectPool<object>.Create(Factory, OnRented);

			// Act
			newPool.Rent();

			// Assert
			Assert.That(called, Is.True);

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
			using ObjectPool<object> newPool = ObjectPool<object>.Create(Factory, onReturn: OnReturned);

			// Act
			object item = newPool.Rent();
			newPool.Return(item);

			// Assert
			Assert.That(called, Is.True);

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
			ObjectPool<object> newPool = ObjectPool<object>.Create(Factory, onDispose: OnDisposed);
			newPool.PreWarm(16);

			// Act
			newPool.Dispose();

			// Assert
			Assert.That(called, Is.True);

			void OnDisposed(object obj)
			{
				called = true;
			}
		}

		private static object Factory()
		{
			return new object();
		}
	}
}