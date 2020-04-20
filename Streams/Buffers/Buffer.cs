using System;
using System.IO;

namespace Stringier.Streams.Buffers {
	/// <summary>
	/// Represents the base of all <see cref="TextStream"/> buffers.
	/// </summary>
	public abstract class Buffer : IReadBuffer, IWriteBuffer {
		/// <summary>
		/// The stream being buffered.
		/// </summary>
		private Stream? stream;

		/// <inheritdoc/>
		public abstract Boolean CanRead { get; }

		/// <inheritdoc/>
		public abstract Boolean CanSeek { get; }

		/// <inheritdoc/>
		public abstract Boolean CanWrite { get; }

		/// <inheritdoc/>
		public virtual Int32 Length { get; set; }

		/// <inheritdoc/>
		public Int32 Position { get; set; }

		/// <inheritdoc/>
		protected internal virtual Stream Stream {
			get {
				if (stream is null) {
					throw new InvalidOperationException("Buffer not associated with stream");
				}
				return stream;
			}
			set {
				if (stream is null) {
					stream = value;
				} else {
					throw new InvalidOperationException("Buffer already associated");
				}
			}
		}

		/// <inheritdoc/>
		Stream IBuffer.Stream {
			get => Stream;
			set => Stream = value;
		}

		/// <inheritdoc/>
		public abstract Boolean Equals(Byte[] other);

		/// <inheritdoc/>
		public Int32 Get() {
			Int32 result = Peek();
			if (Length > 1) {
				Shift(1);
			} else {
				Length = 0;
			}
			return result;
		}

		/// <inheritdoc/>
		public abstract Int32 Peek();

		/// <inheritdoc/>
		public abstract void Read();

		/// <inheritdoc/>
		public virtual void Read(Int32 amount) {
			for (Int32 i = 0; i < amount; i++) {
				Read();
			}
		}

		/// <inheritdoc/>
		public abstract void Shift(Int32 amount);
	}
}
