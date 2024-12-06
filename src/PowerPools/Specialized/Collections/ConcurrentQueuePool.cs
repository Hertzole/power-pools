using System.Collections.Concurrent;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of <see cref="ConcurrentQueue{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of elements in the concurrent queue.</typeparam>
	public sealed class ConcurrentQueuePool<T> : CollectionPool<ConcurrentQueue<T>>
	{
		/// <summary>
		///     Retrieves a shared <see cref="ConcurrentQueuePool{T}" /> instance.
		/// </summary>
		public static ConcurrentQueuePool<T> Shared { get; } = new ConcurrentQueuePool<T>();

		private ConcurrentQueuePool() : base(static () => new ConcurrentQueue<T>(), static queue => queue.Clear()) { }

		/// <summary>
		///     Creates a new instance of <see cref="ConcurrentQueuePool{T}" />.
		/// </summary>
		/// <returns>A new instance of <see cref="ConcurrentQueuePool{T}" />.</returns>
		public static ConcurrentQueuePool<T> Create()
		{
			return new ConcurrentQueuePool<T>();
		}
	}
}