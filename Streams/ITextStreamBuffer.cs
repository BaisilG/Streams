using System;
using System.Collections.Generic;
using System.IO;

namespace Stringier.Streams {
	/// <summary>
	/// Represents a buffer for <see cref="TextStream"/>.
	/// </summary>
	public interface ITextStreamBuffer : IEquatable<Byte[]> {
		/// <summary>
		/// The length of the buffer; the amount of bytes it is currently holding.
		/// </summary>
		public Int32 Length { get; set; }

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

		/// <summary>
		/// Gets the first byte in the buffer, then shifts it off.
		/// </summary>
		/// <returns>The byte, cast to a <see cref="Int32"/>.</returns>
		public Int32 Get();

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
		/// Peeks at the first byte in the buffer.
		/// </summary>
		/// <returns>The byte, cast to a <see cref="Int32"/>.</returns>
		public Int32 Peek();

		/// <summary>
		/// Reads a byte into the buffer.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		public void Read(Stream stream);

		/// <summary>
		/// Reads <paramref name="amount"/> bytes into the buffer.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		/// <param name="amount">The amount of bytes to read.</param>
		public void Read(Stream stream, Int32 amount);

		/// <summary>
		/// Shifts the contents of the buffer left by <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of bytes to shift.</param>
		/// <remarks>
		/// This does not necessarily actually shift the contents, but will appear to.
		/// </remarks>
		public void ShiftLeft(Int32 amount);
	}
}
