namespace Hertzole.PowerPools
{
	/// <summary>
	///     Extensions for <see cref="ObjectPool{T}" />.
	/// </summary>
	public static class ObjectPoolExtensions
	{
		/// <summary>
		///     Retrieves an item from the pool using a <see cref="PoolScope{T}" />. If the pool is empty, a new item will be
		///     created.
		/// </summary>
		/// <remarks>
		///     The <see cref="PoolScope{T}" /> needs to be disposed to return the item to the pool. Consider using this method
		///     together with a <c>using</c> statement.
		/// </remarks>
		/// <param name="pool">The pool to rent from.</param>
		/// <param name="item">The item that was rented.</param>
		/// <typeparam name="T">The type of object to rent.</typeparam>
		/// <returns>A new <see cref="PoolScope{T}" /> instance that needs to be disposed when the caller is done with the item.</returns>
		public static PoolScope<T> Rent<T>(this IObjectPool<T> pool, out T item) where T : class
		{
			item = pool.Rent();
			return new PoolScope<T>(pool, item);
		}
	}
}