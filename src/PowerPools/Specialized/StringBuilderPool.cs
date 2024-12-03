using System;
using System.Text;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool for <see cref="StringBuilder" /> instances using <see cref="ObjectPool{T}" />. It will
	///     automatically clear the <see cref="StringBuilder" /> when it's returned to the pool.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	public sealed class StringBuilderPool : IObjectPool<StringBuilder>
	{
		private readonly int defaultCapacity;
		private readonly ConfigurableObjectPool<StringBuilder> pool;

		/// <summary>
		///     Retrieves a shared <see cref="StringBuilderPool" /> instance.
		/// </summary>
		public static StringBuilderPool Shared { get; } = new StringBuilderPool(OnCreateStatic);

		/// <inheritdoc />
		public int Capacity
		{
			get { return pool.Capacity; }
		}
		/// <inheritdoc />
		public int InPool
		{
			get { return pool.InPool; }
		}

		/// <inheritdoc />
		public int InUse
		{
			get { return pool.InUse; }
		}

		private StringBuilderPool(Func<StringBuilder> factory)
		{
			// For whatever reason I can't use the OnReturn method here without code coverage creating a new inaccessible branch.
			pool = new ConfigurableObjectPool<StringBuilder>(factory, onReturn: static sb => sb.Clear());
		}

		private StringBuilderPool(int defaultCapacity = DEFAULT_CAPACITY)
		{
			this.defaultCapacity = defaultCapacity;
			pool = new ConfigurableObjectPool<StringBuilder>(OnCreate, onReturn: OnReturn);
		}

		internal const int DEFAULT_CAPACITY = 256;

		/// <inheritdoc />
		public StringBuilder Rent()
		{
			return pool.Rent();
		}

		/// <inheritdoc />
		public void Return(StringBuilder item)
		{
			pool.Return(item);
		}

		/// <inheritdoc />
		public int PreWarm(int count)
		{
			return pool.PreWarm(count);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			pool.Dispose();
		}

		private StringBuilder OnCreate()
		{
			return new StringBuilder(defaultCapacity);
		}

		private static StringBuilder OnCreateStatic()
		{
			return new StringBuilder(DEFAULT_CAPACITY);
		}

		private static void OnReturn(StringBuilder item)
		{
			item.Clear();
		}

		/// <summary>
		///     Creates a new instance of <see cref="StringBuilderPool" /> using the default options.
		/// </summary>
		/// <param name="defaultCapacity">The default capacity of the <see cref="StringBuilder" /> instances.</param>
		/// <returns>A new <see cref="StringBuilderPool" /> instance.</returns>
		public static StringBuilderPool Create(int defaultCapacity = 256)
		{
			return new StringBuilderPool(defaultCapacity);
		}
	}
}