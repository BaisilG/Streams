using System;
using System.IO;

namespace Stringier.Streams.Buffers {
	/// <summary>
	/// Represents the base of all <see cref="TextStream"/> buffers.
	/// </summary>
	public abstract class Buffer : IEquatable<Byte[]> {
		/// <summary>
		/// The stream being buffered.
		/// </summary>
		internal Stream Stream;

		/// <summary>
		/// Gets a value that indicates whether the current buffer can be read from.
		/// </summary>
		public abstract Boolean CanRead { get; }

		/// <summary>
		/// Gets a value that indicates whether the current buffer can be seeked.
		/// </summary>
		public abstract Boolean CanSeek { get; }

		/// <summary>
		/// Gets a value that indicates whether the current buffer can be written to.
		/// </summary>
		public abstract Boolean CanWrite { get; }

		/// <summary>
		/// The length of the buffer; the amount of bytes it is currently holding.
		/// </summary>
		public virtual Int32 Length { get; set; }

		/// <summary>
		/// Whether data in the buffer is stale.
		/// </summary>
		public Boolean Stale {
			get => Length == 0;
			set {
				if (value) {
					Length = 0;
				}
			}
		}

		/// <inheritdoc/>
		public abstract Boolean Equals(Byte[] other);

		/// <summary>
		/// Gets the first byte in the buffer, then shifts it off.
		/// </summary>
		/// <returns>The byte, cast to a <see cref="Int32"/>.</returns>
		public Int32 Get() {
			Int32 result = Peek();
			if (Length > 1) {
				ShiftLeft(1);
			} else {
				Length = 0;
			}
			return result;
		}

		/// <summary>
		/// Gets the first byte in the buffer, then shifts it off.
		/// </summary>
		/// <param name="first">The byte, cast to a <see cref="Int32"/>.</param>
		public void Get(out Int32 first) => first = Get();

		/// <summary>
		/// Gets the first two bytes in the buffer, then shifts them off.
		/// </summary>
		/// <param name="first">The first byte, cast to a <see cref="Int32"/>.</param>
		/// <param name="second">The second byte, cast to a <see cref="Int32"/>.</param>
		public void Get(out Int32 first, out Int32 second) {
			first = Get();
			second = Get();
		}

		/// <summary>
		/// Gets the first three bytes in the buffer, then shifts them off.
		/// </summary>
		/// <param name="first">The first byte, cast to a <see cref="Int32"/>.</param>
		/// <param name="second">The second byte, cast to a <see cref="Int32"/>.</param>
		/// <param name="third">The third byte, cast to a <see cref="Int32"/>.</param>
		public void Get(out Int32 first, out Int32 second, out Int32 third) {
			first = Get();
			second = Get();
			third = Get();
		}

		/// <summary>
		/// Gets the first four bytes in the buffer, then shifts them off.
		/// </summary>
		/// <param name="first">The first byte, cast to a <see cref="Int32"/>.</param>
		/// <param name="second">The second byte, cast to a <see cref="Int32"/>.</param>
		/// <param name="third">The third byte, cast to a <see cref="Int32"/>.</param>
		/// <param name="fourth">The fourth byte, cast to a <see cref="Int32"/>.</param>
		public void Get(out Int32 first, out Int32 second, out Int32 third, out Int32 fourth) {
			first = Get();
			second = Get();
			third = Get();
			fourth = Get();
		}

		/// <summary>
		/// Gets the length of <paramref name="buffer"/> bytes from this <see cref="Buffer"/>.
		/// </summary>
		/// <param name="buffer">The buffer to fill.</param>
		public virtual Int32 Get(Span<Byte> buffer) {
			if (Stale) {
				Read(buffer.Length);
			}
			if (Length < buffer.Length) {
				//This scenario sucks. We have to read partially from the buffer, and the remaining amount from the stream
				Int32 val;
				Int32 i = 0;
				Int32 r = buffer.Length;
				// Read from the buffer
				while (i < Length && r-- > 0) {
					val = Get();
					buffer[i++] = val == -1 ? (Byte)0x00 : (Byte)val;
				}
				// Read from the stream
				while (i < buffer.Length && r-- > 0) {
					Get(out val);
					buffer[i++] = val == -1 ? (Byte)0x00 : (Byte)val;
				}
				return i;
			} else {
				//This scenario isn't too bad. The buffer is the same size or larger than what we want to read, so read that part of the buffer.
				Int32 first;
				Int32 second;
				Int32 third;
				Int32 fourth;
				switch (buffer.Length) {
				case 1:
					Get(out first);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					return 1;
				case 2:
					Get(out first, out second);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					buffer[1] = second == -1 ? (Byte)0x00 : (Byte)second;
					return 2;
				case 3:
					Get(out first, out second, out third);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					buffer[1] = second == -1 ? (Byte)0x00 : (Byte)second;
					buffer[2] = third == -1 ? (Byte)0x00 : (Byte)third;
					return 3;
				case 4:
					Get(out first, out second, out third, out fourth);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					buffer[1] = second == -1 ? (Byte)0x00 : (Byte)second;
					buffer[2] = third == -1 ? (Byte)0x00 : (Byte)third;
					buffer[3] = fourth == -1 ? (Byte)0x00 : (Byte)fourth;
					return 4;
				default:
					return 0;
				}
			}
		}

		/// <summary>
		/// Peeks at the first byte in the buffer.
		/// </summary>
		/// <returns>The byte, cast to a <see cref="Int32"/>.</returns>
		/// <remarks>
		/// If the buffer is stale, this also attempts to read into the buffer. The way this is done depends on the buffering strategy.
		/// </remarks>
		public abstract Int32 Peek();

		/// <summary>
		/// Reads a byte into the buffer.
		/// </summary>
		/// <remarks>
		/// This reads at least one byte into the buffer, but the exact amount depends on the buffering strategy.
		/// </remarks>
		public abstract void Read();

		/// <summary>
		/// Reads <paramref name="amount"/> bytes into the buffer.
		/// </summary>
		/// <param name="amount">The amount of bytes to read.</param>
		/// <remarks>
		/// This reads at least <paramref name="amount"/> byte into the buffer, but the exact amount depends on the buffering strategy.
		/// </remarks>
		public virtual void Read(Int32 amount) {
			for (Int32 i = 0; i < amount; i++) {
				Read();
			}
		}

		/// <summary>
		/// Shifts the contents of the buffer left by <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of bytes to shift.</param>
		/// <remarks>
		/// This is used to move the current position within the buffer. It does not necessarily actually shift the contents, but will appear to.
		/// </remarks>
		public abstract void ShiftLeft(Int32 amount);
	}
}
