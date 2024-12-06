using System.Collections.Generic;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool that enables reusing instances of <see cref="List{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	public sealed class ListPool<T> : CollectionPool<List<T>>
	{
		/// <summary>
		///     Retrieves a shared <see cref="ListPool{T}" /> instance.
		/// </summary>
		public static ListPool<T> Shared { get; } = new ListPool<T>();

		private ListPool() : base(static () => new List<T>(16), static list => list.Clear()) { }

		/// <summary>
		///     Creates a new instance of <see cref="ListPool{T}" />.
		/// </summary>
		/// <returns>A new instance of <see cref="ListPool{T}" />.</returns>
		public static ListPool<T> Create()
		{
			return new ListPool<T>();
		}
	}
}