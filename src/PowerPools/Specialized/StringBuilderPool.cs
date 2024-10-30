using System.Text;

namespace Hertzole.PowerPools
{
	/// <summary>
	///     Provides a resource pool for <see cref="StringBuilder" /> instances using <see cref="ObjectPool{T}" />.
	/// </summary>
	/// <remarks>This class is thread-safe. All members may be used by multiple threads concurrently.</remarks>
	public sealed class StringBuilderPool : ObjectPool<StringBuilder>
	{
		private readonly int defaultCapacity;
		private readonly ObjectPool<StringBuilder> pool;

		/// <summary>
		///     Retrieves a shared <see cref="StringBuilderPool" /> instance.
		/// </summary>
		public new static ObjectPool<StringBuilder> Shared { get; } = new SharedObjectPool<StringBuilder>(OnCreateStatic, null, OnReturn);

		/// <inheritdoc />
		public override int Capacity
		{
			get { return pool.Capacity; }
		}
		/// <inheritdoc />
		public override int InPool
		{
			get { return pool.InPool; }
		}

		/// <inheritdoc />
		public override int InUse
		{
			get { return pool.InUse; }
		}

		private StringBuilderPool(int defaultCapacity = 256)
		{
			this.defaultCapacity = defaultCapacity;
			pool = Create(OnCreate, null, OnReturn);
		}

		/// <inheritdoc />
		public override StringBuilder Rent()
		{
			return pool.Rent();
		}

		/// <inheritdoc />
		public override void Return(StringBuilder item)
		{
			pool.Return(item);
		}

		/// <inheritdoc />
		public override int PreWarm(int count)
		{
			return pool.PreWarm(count);
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				pool.Dispose();
			}
		}

		private StringBuilder OnCreate()
		{
			return new StringBuilder(defaultCapacity);
		}

		private static StringBuilder OnCreateStatic()
		{
			return new StringBuilder(256);
		}

		private static void OnReturn(StringBuilder item)
		{
			item.Clear();
		}

		/// <summary>
		///     Creates a new instance of <see cref="StringBuilderPool" /> using the default options.
		/// </summary>
		/// <returns>A new <see cref="StringBuilderPool" /> instance.</returns>
		public static StringBuilderPool Create(int defaultCapacity = 256)
		{
			return new StringBuilderPool(defaultCapacity);
		}
	}
}