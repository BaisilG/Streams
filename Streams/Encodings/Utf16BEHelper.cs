using System;

namespace Stringier.Streams {
	internal sealed class Utf16BEHelper : Utf16Helper {
		public static Byte[] BOM => new Byte[] { 0xFE, 0xFF };

		/// <inheritdoc/>
		public override Encoding Enum => Encoding.UTF16BE;

		/// <inheritdoc/>
		public override Int32 ReadChar() {
			Int32 big = Stream.ReadByte();
			Int32 little = Stream.ReadByte();
			if (big == -1 || little == -1) {
				return -1;
			}
			little += big << 8;
			return little;
		}

		/// <inheritdoc/>
		public override void WriteChar(Char value) {
			Stream.WriteByte((Byte)(value >> 8));
			Stream.WriteByte((Byte)value);
		}
	}
}