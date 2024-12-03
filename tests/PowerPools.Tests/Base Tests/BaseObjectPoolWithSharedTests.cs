using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	public abstract class BaseObjectPoolWithSharedTests<TPool, TItem> : BaseObjectPoolTests<TPool, TItem> where TPool : IObjectPool<TItem> where TItem : class
	{
		protected abstract IObjectPool<TItem> GetShared();

		[Test]
		public void Shared_IsShared()
		{
			// Arrange
			IObjectPool<TItem> shared = GetShared();

			// Assert
			Assert.That(shared, Is.Not.Null);
			Assert.That(shared, Is.SameAs(GetShared()));
			Assert.That(shared, Is.InstanceOf<IObjectPool<TItem>>());
		}
	}
}