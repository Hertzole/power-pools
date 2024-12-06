using System.Collections.Generic;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of <see cref="Stack{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of elements in the stack.</typeparam>
	public sealed class StackPool<T> : CollectionPool<Stack<T>>
	{
		/// <summary>
		///     Retrieves a shared <see cref="StackPool{T}" /> instance.
		/// </summary>
		public static StackPool<T> Shared { get; } = new StackPool<T>();

		private StackPool() : base(static () => new Stack<T>(16), static stack => stack.Clear()) { }

		/// <summary>
		///     Creates a new instance of <see cref="StackPool{T}" />.
		/// </summary>
		/// <returns>A new instance of <see cref="StackPool{T}" />.</returns>
		public static StackPool<T> Create()
		{
			return new StackPool<T>();
		}
	}
}