namespace Hertzole.PowerPools
{
	/// <summary>
	///     Exception thrown when the pool is full.
	/// </summary>
	public sealed class PoolFullException : PoolException
	{
		private static readonly string message =
			"Can't return more items to the pool because the pool is full. You can't return more items than the pool's capacity.";

		/// <summary>
		///     Initializes a new instance of the <see cref="PoolFullException" /> class.
		/// </summary>
		internal PoolFullException() : base(message) { }

		internal PoolFullException(string message) : base(message) { }
	}
}