using System;

namespace Stringier.Streams {
	internal sealed class Utf16LEHelper : Utf16Helper {
		public static Byte[] BOM => new Byte[] { 0xFF, 0xFE };

		/// <inheritdoc/>
		public override Encoding Enum => Encoding.UTF16LE;

		/// <inheritdoc/>
		public override Int32 ReadChar() {
			Int32 little = Stream.ReadByte();
			Int32 big = Stream.ReadByte();
			if (little == -1 || big == -1) {
				return -1;
			}
			little += big << 8;
			return little;
		}
	}
}
