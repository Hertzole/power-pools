using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Base interface for object pools.
	/// </summary>
	/// <typeparam name="T">The type of object to pool.</typeparam>
	public interface IObjectPool<T> : IDisposable where T : class
	{
		/// <summary>
		///     Returns the amount of items that can be stored in the pool before it needs to resize.
		/// </summary>
		int Capacity { get; }
		/// <summary>
		///     Returns the amount of items that are currently in the pool.
		/// </summary>
		int InPool { get; }
		/// <summary>
		///     Returns the amount of items that have been taken from the pool and not returned.
		/// </summary>
		int InUse { get; }

		/// <summary>
		///     Retrieves an item from the pool. If the pool is empty, a new item will be created.
		/// </summary>
		/// <remarks>
		///     The item should be returned to the pool when it's no longer needed via <see cref="Return" /> so that it can be
		///     reused. It's not a fatal error to not return an item, but failure to do so may lead to decreased application
		///     performance.
		/// </remarks>
		/// <returns>A newly created item or a reused item.</returns>
		T Rent();

		/// <summary>
		///     Returns an item to the pool that was previously obtained via <see cref="Rent" />.
		/// </summary>
		/// <remarks>
		///     Once the item has been returned, it should not be used anymore. Doing so may result in unexpected behavior.
		/// </remarks>
		/// <param name="item">The item to return to the pool.</param>
		void Return(T item);

		/// <summary>
		///     Ensures that the pool has at least the specified amount of items in it.
		/// </summary>
		/// <param name="count">The amount of items to ensure are in the pool.</param>
		/// <returns>The amount of items that were created.</returns>
		int PreWarm(int count);
	}
}