using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Hertzole.PowerPools
{
	internal sealed class PooledStack<T> : IDisposable
	{
		private T[] items;

		public int Length { get; private set; }
		public int Capacity
		{
			get { return items.Length; }
		}

		public PooledStack(int capacity)
		{
			items = ArrayPool<T>.Shared.Rent(capacity);
			Length = 0;
		}

		public void Push(T item)
		{
			int size = Length;

			if ((uint) size >= (uint) items.Length)
			{
				Grow(size + 1);
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

		private void Grow(int capacity)
		{
			int newCapacity = items.Length == 0 ? 16 : items.Length * 2;

			if ((uint) newCapacity > int.MaxValue)
			{
				newCapacity = int.MaxValue;
			}

			if (newCapacity < capacity)
			{
				newCapacity = capacity;
			}

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