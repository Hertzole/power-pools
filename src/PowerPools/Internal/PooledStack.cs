using System;
using System.Buffers;
using System.Diagnostics;

namespace Hertzole.PowerPools
{
	internal sealed class PooledStack<T> : IDisposable where T : class
	{
		internal T[] items;

		public int Length { get; private set; }
		public int Capacity
		{
			get { return items?.Length ?? 0; }
		}

		public PooledStack(int capacity = 16)
		{
			Debug.Assert(capacity > 0, "Capacity must be greater than 0.");

			items = ArrayPool<T>.Shared.Rent(capacity);
			Length = 0;
		}

		internal T this[uint index]
		{
			get { return items[index]; }
		}

		public void Push(T item)
		{
			int size = Length;

			if ((uint) size >= (uint) items.Length)
			{
				Grow();
			}

			items[size] = item;
			Length = size + 1;
		}

		public bool TryPop(out T result)
		{
			if (Length == 0)
			{
				result = default;
				return false;
			}

			int size = Length - 1;
			result = items[size];
			Length = size;
			// No need to check only if this item is a reference/unmanaged type since we'll always be using references.
			items[size] = default;

			return true;
		}

		private void Grow()
		{
			int newCapacity = items.Length * 2;

			if (newCapacity != items.Length)
			{
				T[] newArray = ArrayPool<T>.Shared.Rent(newCapacity);
				Array.Copy(items, newArray, Length);
				ArrayPool<T>.Shared.Return(items, true);
				items = newArray;
			}
		}

		public void Dispose()
		{
			ArrayPool<T>.Shared.Return(items, true);
			items = null;
			Length = 0;
		}
	}
}