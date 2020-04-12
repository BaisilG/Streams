using System;
using System.Collections.Generic;
using System.Text;

namespace Stringier.Streams {
	internal sealed class Utf32LE : EncodingHelper {
		public static Byte[] BOM => new Byte[] { 0xFF, 0xFE, 0x00, 0x00 };

		/// <inheritdoc/>
		public override Encoding Enum => Encoding.UTF32LE;

		/// <inheritdoc/>
		public override Int32 ReadChar(TextStream stream) => throw new NotImplementedException();

		/// <inheritdoc/>
		public override Int32 ReadRune(TextStream stream) => throw new NotImplementedException();
	}
}
