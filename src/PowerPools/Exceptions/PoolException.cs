using System;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Base exception for all pool related exceptions.
	/// </summary>
	public class PoolException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="PoolException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		internal PoolException(string message) : base(message) { }
	}
}