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
				public override Byte[] BOM => new Byte[] { 0x00, 0x00, 0xFE, 0xFF };

				/// <inheritdoc/>
				public override Encoding Enum => Encoding.UTF32BE;

				/// <inheritdoc/>
				public override Int32 ReadChar(TextStream stream) => throw new NotImplementedException();

				/// <inheritdoc/>
				public override Int32 ReadRune(TextStream stream) => throw new NotImplementedException();
			}
		}
	}
}