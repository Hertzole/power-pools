using System.Collections.Generic;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of <see cref="Queue{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of elements in the queue.</typeparam>
	public sealed class QueuePool<T> : CollectionPool<Queue<T>>
	{
		/// <summary>
		///     Retrieves a shared <see cref="QueuePool{T}" /> instance.
		/// </summary>
		public static QueuePool<T> Shared { get; } = new QueuePool<T>();

		private QueuePool() : base(static () => new Queue<T>(16), static queue => queue.Clear()) { }

		/// <summary>
		///     Creates a new instance of <see cref="QueuePool{T}" />.
		/// </summary>
		/// <returns>A new instance of <see cref="QueuePool{T}" />.</returns>
		public static QueuePool<T> Create()
		{
			return new QueuePool<T>();
		}
	}
}