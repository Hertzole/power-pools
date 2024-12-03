using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class ObjectPoolTests : BaseObjectPoolTests<ObjectPool<object>, object>
	{
		protected override ObjectPool<object> CreatePool(int capacity, Action<object>? onRent = null, Action<object>? onReturn = null, Action<object>? onDispose = null)
		{
			return ObjectPool<object>.Create(Factory, onRent, onReturn, onDispose, capacity);
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

		private static object Factory()
		{
			return new object();
		}
	}
}