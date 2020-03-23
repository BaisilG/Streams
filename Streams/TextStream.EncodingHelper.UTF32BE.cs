using System;
using System.Collections.Generic;
using System.Text;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// UTF-32 big endian
			/// </summary>
			public static EncodingHelper UTF32BE { get; } = new UTF32BEEncodingHelper();

			internal sealed class UTF32BEEncodingHelper : EncodingHelper {
				/// <summary>
				/// The <see cref="Encoding"/> enum representing this helper.
				/// </summary>
				public override Encoding Enum => Encoding.UTF32BE;

				public UTF32BEEncodingHelper() : base(new Byte[] { 0x00, 0x00, 0xFE, 0xFF }) {

				}
			}
		}
	}
}