using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	public abstract class BaseObjectPoolWithSharedTests<TPool, TItem> : BaseObjectPoolTests<TPool, TItem> where TPool : ObjectPool<TItem> where TItem : class, new()
	{
		protected abstract ObjectPool<TItem> GetShared();

		[Test]
		public void Shared_IsShared()
		{
			// Arrange
			ObjectPool<object> shared = ObjectPool<object>.Shared;

			// Assert
			Assert.That(shared, Is.SameAs(ObjectPool<object>.Shared));
			Assert.That(shared, Is.TypeOf<SharedObjectPool<object>>());
		}
	}
}