using System;
using System.IO;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		/// <summary>
		/// Provides a safe and reusable buffer for <see cref="TextStream"/>.
		/// </summary>
		/// <remarks>
		/// This entire thing is required for peeking and reading the BOM. It's not a performance buffer, so it's very small.
		/// </remarks>
		internal class Buffer : IReadBuffer, IWriteBuffer {
			private readonly Byte[] buffer = new Byte[4];

			/// <inheritdoc/>
			public Boolean CanRead { get; }

			/// <inheritdoc/>
			public Boolean CanSeek { get; }

			/// <inheritdoc/>
			public Boolean CanWrite { get; }

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
			public Boolean Equals(Byte[] other) {
				if (Length < other.Length) {
					return false;
				}
				for (Int32 i = 0; i < other.Length; i++) {
					if (buffer[i] != other[i]) {
						return false;
					}
				}
				return true;
			}

			/// <inheritdoc/>
			public Int32 Get() {
				Int32 result = Peek();
				if (Length > 1) {
					ShiftLeft(1);
				} else {
					Length = 0;
				}
				return result;
			}

			/// <inheritdoc/>
			public Int32 Peek() => Length > 0 ? buffer[0] : -1;

			/// <inheritdoc/>
			public void Read(Stream stream) {
				Int32 read = stream.ReadByte();
				if (read >= 0) {
					buffer[Length++] = (Byte)read;
				}
			}

			/// <inheritdoc/>
			public void Read(Stream stream, Int32 amount) {
				for (Int32 i = 0; i < amount; i++) {
					Read(stream);
				}
			}

			/// <inheritdoc/>
			public void ShiftLeft(Int32 amount) {
				if (amount > 4) {
					amount = 4;
				}
				for (Int32 i = 0; i < amount; i++) {
					buffer[0] = buffer[1];
					buffer[1] = buffer[2];
					buffer[2] = buffer[3];
					Length--;
				}
			}

			/// <inheritdoc/>
			public override String ToString() => $"[{buffer[0]}, {buffer[1]}, {buffer[2]}, {buffer[3]}]";
		}
	}
}
