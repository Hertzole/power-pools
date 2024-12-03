using Hertzole.PowerPools;

namespace PowerPools.Tests
{
	[TestFixture]
	public class PooledStackTests
	{
		private PooledStack<object> stack = null!;

		[SetUp]
		public void SetUp()
		{
			stack = new PooledStack<object>();
		}

		[TearDown]
		public void TearDown()
		{
			stack.Dispose();
		}

		[Test]
		public void CreateCapacity_HasAtLeastCapacity([Values(1, 5, 10, 100)] int capacity)
		{
			// Arrange
			using PooledStack<object> newStack = new PooledStack<object>(capacity);

			// Assert
			Assert.That(newStack.Capacity, Is.AtLeast(capacity));
		}

		[Test]
		public void Create_HasZeroLength()
		{
			// Arrange
			using PooledStack<object> newStack = new PooledStack<object>(100);

			// Assert
			Assert.That(newStack.Length, Is.Zero);
		}

		[Test]
		public void Dispose_ReturnsPool()
		{
			// Arrange
			PooledStack<object> newStack = new PooledStack<object>(1);
			newStack.Push(new object());

			// Act
			newStack.Dispose();

			// Assert
			Assert.That(newStack.Length, Is.Zero);
			Assert.That(newStack.items, Is.Null);
		}

		[Test]
		public void Push_StackHasItem()
		{
			// Arrange
			object item = new object();

			// Act
			stack.Push(item);

			// Assert
			Assert.That(stack.Length, Is.EqualTo(1));
			Assert.That(stack.items[0], Is.EqualTo(item));
		}

		[Test]
		public void Push_StackGrows()
		{
			// Arrange
			object item = new object();

			// Act
			for (int i = 0; i < 20; i++)
			{
				stack.Push(item);
			}

			// Assert
			Assert.That(stack.Length, Is.EqualTo(20));
			Assert.That(stack.Capacity, Is.AtLeast(20));
		}

		[Test]
		public void TryPop_StackIsEmpty_ReturnsFalse()
		{
			// Act
			bool result = stack.TryPop(out object item);

			// Assert
			Assert.That(result, Is.False);
			Assert.That(item, Is.Null);
		}

		[Test]
		public void TryPop_StackHasItem_ReturnsTrue()
		{
			// Arrange
			object item = new object();
			stack.Push(item);

			// Act
			bool result = stack.TryPop(out object resultItem);

			// Assert
			Assert.That(result, Is.True);
			Assert.That(resultItem, Is.EqualTo(item));
			Assert.That(stack.Length, Is.Zero);
			Assert.That(stack.items, Does.Not.Contain(item));
		}
	}
}