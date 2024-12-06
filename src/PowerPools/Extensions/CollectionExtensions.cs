#if !NETCOREAPP2_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER // This is mainly used for ConcurrentQueue.Clear() which is only available in .NET Core 2.0+ and .NET Standard 2.1+
using System.Collections.Concurrent;

namespace Hertzole.PowerPools
{
	internal static class CollectionExtensions
	{
		public static void Clear<T>(this ConcurrentQueue<T> queue)
		{
			while (queue.TryDequeue(out _)) { }
		}
	}
}
#endif