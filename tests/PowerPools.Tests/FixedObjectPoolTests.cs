using System;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class FixedObjectPoolTests : BaseFixedSizeObjectPoolTests<FixedSizeObjectPool<object>, object>
	{
		protected override FixedSizeObjectPool<object> CreatePool(int capacity,
			Action<object>? onRent = null,
			Action<object>? onReturn = null,
			Action<object>? onDispose = null)
		{
			return FixedSizeObjectPool<object>.Create(capacity, Factory, onRent, onReturn, onDispose);
		}
		
		[Test]
		public void Create_WithNullFactory_ThrowsArgumentNullException()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => FixedSizeObjectPool<object>.Create(16, null!));
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

		protected override object Factory()
		{
			return new object();
		}
	}
}