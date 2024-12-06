using System.Collections.Concurrent;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of <see cref="ConcurrentStack{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of elements in the concurrent stack.</typeparam>
	public sealed class ConcurrentStackPool<T> : CollectionPool<ConcurrentStack<T>>
	{
		/// <summary>
		///     Retrieves a shared <see cref="ConcurrentStackPool{T}" /> instance.
		/// </summary>
		public static ConcurrentStackPool<T> Shared { get; } = new ConcurrentStackPool<T>();

		private ConcurrentStackPool() : base(static () => new ConcurrentStack<T>(), static stack => stack.Clear()) { }

		/// <summary>
		///     Creates a new instance of <see cref="ConcurrentStackPool{T}" />.
		/// </summary>
		/// <returns>A new instance of <see cref="ConcurrentStackPool{T}" />.</returns>
		public static ConcurrentStackPool<T> Create()
		{
			return new ConcurrentStackPool<T>();
		}
	}
}