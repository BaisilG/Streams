using System;
using System.Text;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		/// <summary>
		/// The encoding helper for a <see cref="TextStream"/>.
		/// </summary>
		/// <remarks>
		/// This exists only to help manage read/write operations from <see cref="TextStream"/> in a maintainable and scalable way.
		/// </remarks>
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// The byte-order-mark of this <see cref="Encoding"/>.
			/// </summary>
			public abstract Byte[] BOM { get; }

			/// <summary>
			/// The <see cref="Encoding"/> enum representing this helper.
			/// </summary>
			public abstract Encoding Enum { get; }

			/// <summary>
			/// Reads a <see cref="Char"/> from the <paramref name="stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="TextStream"/> to read from.</param>
			/// <returns>The <see cref="Rune"/>, cast to a <see cref="Int32"/> value.</returns>
			public abstract Int32 ReadChar(TextStream stream);

			/// <summary>
			/// Reads a <see cref="Rune"/> from the <paramref name="stream"/>.
			/// </summary>
			/// <param name="stream">The <see cref="TextStream"/> to read from.</param>
			/// <returns>The <see cref="Rune"/>, cast to a <see cref="Int32"/> value.</returns>
			public abstract Int32 ReadRune(TextStream stream);
		}
	}
}