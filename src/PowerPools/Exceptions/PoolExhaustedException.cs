namespace Hertzole.PowerPools
{
	/// <summary>
	///     Exception thrown when the pool is exhausted.
	/// </summary>
	public sealed class PoolExhaustedException : PoolException
	{
		private static readonly string message =
			"Can't rent more items because the pool is exhausted. You can't rent more items than the pool's capacity. Return some items back to the pool before renting more.";

		/// <summary>
		///     Initializes a new instance of the <see cref="PoolExhaustedException" /> class.
		/// </summary>
		internal PoolExhaustedException() : base(message) { }
	}
}