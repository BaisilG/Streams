using System;
using System.Collections.Generic;
using System.Text;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// UTF-16 big endian
			/// </summary>
			public static EncodingHelper UTF16BE { get; } = new UTF16BEEncodingHelper();

			internal sealed class UTF16BEEncodingHelper : EncodingHelper {
				public override Byte[] BOM => new Byte[] { 0xFE, 0xFF };

				/// <inheritdoc/>
				public override Encoding Enum => Encoding.UTF16BE;

				/// <inheritdoc/>
				public override Int32 ReadChar(TextStream stream) => throw new NotImplementedException();

				/// <inheritdoc/>
				public override Int32 ReadRune(TextStream stream) => throw new NotImplementedException();
			}
		}
	}
}