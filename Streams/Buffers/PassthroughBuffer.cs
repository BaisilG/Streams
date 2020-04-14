using System;
using System.IO;

namespace Stringier.Streams {
	/// <summary>
	/// Provides a passthrough buffer for <see cref="TextStream"/>.
	/// </summary>
	/// <remarks>
	/// This entire thing is required for peeking and reading the BOM. It's not a performance buffer, so it's very small. Otherwise, all operations pass-through to the base stream, such that no buffering is done.
	/// </remarks>
	internal sealed class PassthroughBuffer : IReadBuffer, IWriteBuffer {
		private readonly Byte[] Buffer = new Byte[4];

		/// <inheritdoc/>
		public Boolean CanRead => Buffer.Length > 0;

		/// <inheritdoc/>
		public Boolean CanSeek => Buffer.Length > 0;

		/// <inheritdoc/>
		public Boolean CanWrite => false;

		/// <inheritdoc/>
		public Int32 Length { get; set; } = 0;

		/// <inheritdoc/>
		public Boolean Stale {
			get => Length == 0;
			set {
				if (value) {
					Length = 0;
				}
			}
		}

		/// <inheritdoc/>
		public Stream Stream { get; set; }

		/// <inheritdoc/>
		public Boolean Equals(Byte[] other) {
			if (Length < other.Length) {
				return false;
			}
			for (Int32 i = 0; i < other.Length; i++) {
				if (Buffer[i] != other[i]) {
					return false;
				}
			}
			return true;
		}

		/// <inheritdoc/>
		public Int32 Peek() => Length > 0 ? Buffer[0] : -1;

		/// <inheritdoc/>
		public void Read() {
			Int32 read = Stream.ReadByte();
			if (read >= 0) {
				Buffer[Length++] = (Byte)read;
			}
		}

		/// <inheritdoc/>
		public void ShiftLeft(Int32 amount) {
			if (amount > 4) {
				amount = 4;
			}
			for (Int32 i = 0; i < amount; i++) {
				Buffer[0] = Buffer[1];
				Buffer[1] = Buffer[2];
				Buffer[2] = Buffer[3];
				Length--;
			}
		}

		/// <inheritdoc/>
		public override String ToString() => $"[{Buffer[0]}, {Buffer[1]}, {Buffer[2]}, {Buffer[3]}]";
	}
}