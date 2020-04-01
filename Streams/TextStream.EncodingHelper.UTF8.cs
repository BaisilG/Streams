using System;
using System.Collections.Generic;
using System.Text;
using Stringier.Encodings;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// UTF-8
			/// </summary>
			public static EncodingHelper UTF8 { get; } = new UTF8EncodingHelper();

			internal sealed class UTF8EncodingHelper : EncodingHelper {
				/// <inheritdoc/>
				public override Byte[] BOM => new Byte[] { 0xEF, 0xBB, 0xBF };

				/// <inheritdoc/>
				public override Encoding Enum => Encoding.UTF8;

				/// <inheritdoc/>
				public override Int32 ReadChar(TextStream stream) => throw new NotImplementedException();

				/// <inheritdoc/>
				public override Int32 ReadRune(TextStream stream) => throw new NotImplementedException();
			}
		}
	}
}