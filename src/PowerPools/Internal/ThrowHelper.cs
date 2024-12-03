#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
#define NULLABLE_ANNOTATIONS
#endif

using System;
#if NULLABLE_ANNOTATIONS
using System.Diagnostics.CodeAnalysis;
#endif

namespace Hertzole.PowerPools
{
	internal static class ThrowHelper
	{
		internal static void ThrowIfNull(object argument, string paramName)
		{
			// This returns early because otherwise code coverage won't hit the end of the if block.
			if (argument is not null)
			{
				return;
			}

			ThrowArgumentNullException(paramName);
		}

#if NULLABLE_ANNOTATIONS
		[DoesNotReturn]
#endif
		private static void ThrowArgumentNullException(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}