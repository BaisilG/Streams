using System;
using System.Collections.Generic;
using System.Text;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// UTF-32 little endian
			/// </summary>
			public static EncodingHelper UTF32LE { get; } = new UTF32LEEncodingHelper();

			internal sealed class UTF32LEEncodingHelper : EncodingHelper {
				/// <summary>
				/// The <see cref="Encoding"/> enum representing this helper.
				/// </summary>
				public override Encoding Enum => Encoding.UTF32LE;

				public UTF32LEEncodingHelper() : base(new Byte[] { 0xFF, 0xFE, 0x00, 0x00 }) {

				}
			}
		}
	}
}
