using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Hertzole.PowerPools
{
	internal sealed class PooledStack<T> : IDisposable
	{
		internal T[] items;

		public int Length { get; private set; }
		public int Capacity
		{
			get { return items.Length; }
		}

		public PooledStack(int capacity = 16)
		{
			Debug.Assert(capacity > 0, "Capacity must be greater than 0.");

			items = ArrayPool<T>.Shared.Rent(capacity);
			Length = 0;
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
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				items[size] = default;
			}

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