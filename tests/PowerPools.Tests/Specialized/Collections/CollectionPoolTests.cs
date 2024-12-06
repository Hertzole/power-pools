using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	public abstract class CollectionPoolTests<TPool, TItem> : BaseObjectPoolWithSharedTests<TPool, TItem>
		where TPool : IObjectPool<TItem> where TItem : class
	{
		public override void Create_HasCapacity([Values(0)] int capacity)
		{
			Assert.Pass("Collection pools do not have a capacity.");
		}

		public override void Create_WithRentCallback_CallsCallback()
		{
			Assert.Pass("Rent callbacks are not supported for collection pools.");
		}

		public override void Create_WithReturnCallback_CallsCallback()
		{
			Assert.Pass("Return callbacks are not supported for collection pools.");
		}

		public override void Dispose_WithDisposeCallback_CallsCallback()
		{
			Assert.Pass("Dispose callbacks are not supported for collection pools.");
		}

		[Test]
		public void Rent_CountIsZero()
		{
			// Arrange
			TItem? item = Pool.Rent();
			AddToCollection(item);
			Pool.Return(item);

			// Act
			item = Pool.Rent();

			// Assert
			AssertCount(item, 0);
		}

		[Test]
		public void Dispose_ClearsCollection()
		{
			// Arrange
			TPool pool = CreatePool(16);
			TItem? item = pool.Rent();
			pool.Return(item);
			// Adds items to the collection while it's in the pool.
			AddToCollection(item);

			// Act
			pool.Dispose();

			// Assert
			AssertCount(item, 0);
		}

		protected abstract void AddToCollection(TItem item);

		protected abstract void AssertCount(TItem item, int count);
	}
}