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
				/// <inheritdoc/>
				public override Byte[] BOM => new Byte[] { 0xFF, 0xFE, 0x00, 0x00 };

				/// <inheritdoc/>
				public override Encoding Enum => Encoding.UTF32LE;

				/// <inheritdoc/>
				public override Int32 ReadChar(TextStream stream) => throw new NotImplementedException();

				/// <inheritdoc/>
				public override Int32 ReadRune(TextStream stream) => throw new NotImplementedException();
			}
		}
	}
}
