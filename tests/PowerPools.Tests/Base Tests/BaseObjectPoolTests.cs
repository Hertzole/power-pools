using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	public abstract class BaseObjectPoolTests<TPool, TItem> where TPool : ObjectPool<TItem> where TItem : class, new()
	{
		protected TPool Pool { get; private set; } = null!;
		protected Random Random { get; } = new Random();

		[SetUp]
		public void SetUp()
		{
			Pool = CreatePool();
		}

		[TearDown]
		public void TearDown()
		{
			Pool.Dispose();
		}

		protected abstract TPool CreatePool();

		[Test]
		public void Create_HasZeroInUse()
		{
			// Arrange
			using TPool newPool = CreatePool();

			// Assert
			Assert.That(newPool.InUse, Is.Zero);
		}

		[Test]
		public void Create_HasZeroInPool()
		{
			// Arrange
			using TPool newPool = CreatePool();

			// Assert
			Assert.That(newPool.InPool, Is.Zero);
		}

		[Test]
		public void Create_HasCapacity()
		{
			// Arrange
			using TPool newPool = CreatePool();

			// Assert
			Assert.That(newPool.Capacity, Is.AtLeast(1));
		}

		[Test]
		public void Rent_ReturnsItem()
		{
			// Act
			Pool.Rent();

			// Assert
			Assert.That(Pool.InUse, Is.EqualTo(1));
			Assert.That(Pool.InPool, Is.Zero);
		}

		[Test]
		public void Return_ReturnsPool()
		{
			// Arrange
			TItem item = Pool.Rent();

			// Act
			Pool.Return(item);

			// Assert
			Assert.That(Pool.InUse, Is.Zero);
			Assert.That(Pool.InPool, Is.EqualTo(1));
		}

		[Test]
		public void RentReturn_IsSameItem()
		{
			// Arrange
			TItem item = Pool.Rent();

			// Act
			Pool.Return(item);
			TItem newItem = Pool.Rent();

			// Assert
			Assert.That(newItem, Is.EqualTo(item));
			Assert.That(Pool.InUse, Is.EqualTo(1));
			Assert.That(Pool.InPool, Is.Zero);
		}

		[Test]
		public void RentScope_ReturnsItem()
		{
			// Arrange
			PoolScope<TItem> scope = Pool.Rent(out TItem? item);

			// Act
			scope.Dispose();

			// Assert
			Assert.That(item, Is.Not.Null);
			Assert.That(Pool.InUse, Is.Zero);
			Assert.That(Pool.InPool, Is.EqualTo(1));
		}

		[Test]
		public virtual void PreWarm_PreWarms([Values(1, 5, 10, 100)] int amount)
		{
			// Arrange
			Pool.PreWarm(amount);

			// Assert
			Assert.That(Pool.InPool, Is.EqualTo(amount));
			Assert.That(Pool.Capacity, Is.AtLeast(amount));
		}

		[Test]
		public void PreWarm_ReturnsCreatedItems()
		{
			// Arrange
			TItem obj = Pool.Rent();
			Pool.Return(obj); // Makes sure there's at least one item in the pool before pre-warming.

			// Act
			int amount = Pool.PreWarm(5); // This should've created 4 new items since there's already one in the pool.

			// Assert
			Assert.That(amount, Is.EqualTo(4));
			Assert.That(Pool.InPool, Is.EqualTo(5));
		}

		[Test]
		public void PreWarm_AlreadyWarm_ReturnsZero()
		{
			// Arrange
			Pool.PreWarm(10);

			// Act
			int amount = Pool.PreWarm(5);

			// Assert
			Assert.That(amount, Is.Zero);
			Assert.That(Pool.InPool, Is.EqualTo(10));
		}

		[Test]
		public void Dispose_ReturnsPool()
		{
			// Arrange
			TPool pool = CreatePool();

			// Act
			pool.Dispose();

			// Assert
			Assert.That(pool.InUse, Is.Zero);
			Assert.That(pool.InPool, Is.Zero);
			Assert.That(pool.Capacity, Is.Zero);
		}
	}
}