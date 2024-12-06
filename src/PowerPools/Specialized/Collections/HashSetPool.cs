using System.Collections.Generic;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of <see cref="HashSet{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of elements in the hash set.</typeparam>
	public sealed class HashSetPool<T> : CollectionPool<HashSet<T>>
	{
		/// <summary>
		///     Retrieves a shared <see cref="HashSetPool{T}" /> instance.
		/// </summary>
		public static HashSetPool<T> Shared { get; } = new HashSetPool<T>();

		private HashSetPool() : base(static () => new HashSet<T>(), static set => set.Clear()) { }

		/// <summary>
		///     Creates a new instance of <see cref="HashSetPool{T}" />.
		/// </summary>
		/// <returns>A new instance of <see cref="HashSetPool{T}" />.</returns>
		public static HashSetPool<T> Create()
		{
			return new HashSetPool<T>();
		}
	}
}