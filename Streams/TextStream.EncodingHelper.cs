using System;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		/// <summary>
		/// The encoding helper for a <see cref="TextStream"/>.
		/// </summary>
		/// <remarks>
		/// This exists only to help manage read/write operations from <see cref="TextStream"/> in a maintainable and scalable way.
		/// </remarks>
		internal abstract partial class EncodingHelper {
			protected EncodingHelper(Byte[] bom) {
				BOM = bom;
			}

			public static implicit operator EncodingHelper(Encoding encoding) {
				switch (encoding) {
				case Encoding.UTF8:
					return UTF8;
				default:
					throw new ArgumentException("There isn't a defined helper for this encoding", nameof(encoding));
				}
			}

			/// <summary>
			/// The byte-order-mark of this <see cref="Encoding"/>.
			/// </summary>
			public Byte[] BOM { get; }

			/// <summary>
			/// The <see cref="Encoding"/> enum representing this helper.
			/// </summary>
			public abstract Encoding Enum { get; }
		}
	}
}