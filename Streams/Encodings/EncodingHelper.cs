using System;
using System.Text;

namespace Stringier.Streams {
	/// <summary>
	/// The encoding helper for a <see cref="TextStream"/>.
	/// </summary>
	/// <remarks>
	/// This exists only to help manage read/write operations from <see cref="TextStream"/> in a maintainable and scalable way.
	/// </remarks>
	internal abstract partial class EncodingHelper {
		/// <summary>
		/// The stream.
		/// </summary>
		private TextStream? stream;

		/// <summary>
		/// The <see cref="Encoding"/> enum representing this helper.
		/// </summary>
		public abstract Encoding Enum { get; }

		/// <summary>
		/// The stream.
		/// </summary>
		protected internal TextStream Stream {
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
					throw new InvalidOperationException("Buffer already associated with stream");
				}
			}
		}

		/// <summary>
		/// Reads a <see cref="Char"/>.
		/// </summary>
		/// <returns>The <see cref="Rune"/>, cast to a <see cref="Int32"/> value.</returns>
		public abstract Int32 ReadChar();

		/// <summary>
		/// Reads a <see cref="Rune"/>.
		/// </summary>
		/// <returns>The <see cref="Rune"/>, cast to a <see cref="Int32"/> value.</returns>
		public abstract Int32 ReadRune();
	}
}