using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class ObjectPoolTests : BaseObjectPoolWithSharedTests<ObjectPool<object>, object>
	{
		protected override ObjectPool<object> CreatePool()
		{
			return ObjectPool<object>.Create();
		}

		protected override ObjectPool<object> GetShared()
		{
			return ObjectPool<object>.Shared;
		}

		[Test]
		public void Create_WithFactory_CallsFactory()
		{
			// Arrange
			bool called = false;
			using ObjectPool<object> newPool = ObjectPool<object>.Create(Factory);

			// Act
			newPool.Rent();

			// Assert
			Assert.That(called, Is.True);

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
			using ObjectPool<object> newPool = ObjectPool<object>.Create(onRent: OnRented);

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
			using ObjectPool<object> newPool = ObjectPool<object>.Create(onReturn: OnReturned);

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
			ObjectPool<object> newPool = ObjectPool<object>.Create(onDispose: OnDisposed);
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
	}
}