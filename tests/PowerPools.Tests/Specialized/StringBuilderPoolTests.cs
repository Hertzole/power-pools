using System.Text;
using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class StringBuilderPoolTests : BaseObjectPoolWithSharedTests<StringBuilderPool, StringBuilder>
	{
		protected override StringBuilderPool CreatePool(int capacity)
		{
			// Capacity is not used in the same way with string builder pool.
			return StringBuilderPool.Create();
		}

		protected override IObjectPool<StringBuilder> GetShared()
		{
			return StringBuilderPool.Shared;
		}

		[Test]
		public void Create_HasDefaultCapacity()
		{
			// Arrange
			using StringBuilderPool pool = StringBuilderPool.Create();

			// Act
			StringBuilder item = pool.Rent();

			// Assert
			Assert.That(item.Capacity, Is.AtLeast(StringBuilderPool.DEFAULT_CAPACITY));
		}

		[Test]
		public void Create_WithDefaultCapacity_ItemHasCapacity([Values(1, 5, 100, 1000)] int capacity)
		{
			// Arrange
			using StringBuilderPool pool = StringBuilderPool.Create(capacity);

			// Act
			StringBuilder item = pool.Rent();

			// Assert
			Assert.That(item.Capacity, Is.AtLeast(capacity));
		}

		[Test]
		public void Rent_Shared_HasDefaultCapacity()
		{
			// Act
			StringBuilder item = StringBuilderPool.Shared.Rent();

			// Assert
			Assert.That(item.Capacity, Is.AtLeast(StringBuilderPool.DEFAULT_CAPACITY));
		}
		
		[Test]
		public override void Create_HasCapacity([Values(1, 5, 50, 1000)] int capacity)
		{
			Assert.Pass("StringBuilderPool does not use capacity in the same way as other pools.");
		}
	}
}