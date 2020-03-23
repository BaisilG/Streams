using System;
using System.Collections.Generic;
using System.Text;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// UTF-16 little endian
			/// </summary>
			public static EncodingHelper UTF16LE { get; } = new UTF16LEEncodingHelper();

			internal sealed class UTF16LEEncodingHelper : EncodingHelper {
				/// <summary>
				/// The <see cref="Encoding"/> enum representing this helper.
				/// </summary>
				public override Encoding Enum => Encoding.UTF16LE;

				public UTF16LEEncodingHelper() : base(new Byte[] { 0xFF, 0xFE }) {

				}
			}
		}
	}
}