using System;

namespace Stringier.Streams {
	internal sealed class Utf32BEHelper : Utf32Helper {
		public static Byte[] BOM => new Byte[] { 0x00, 0x00, 0xFE, 0xFF };

		/// <inheritdoc/>
		public override Encoding Enum => Encoding.UTF32BE;

		/// <inheritdoc/>
		public override Int32 ReadRune() {
			Int32 b3 = Stream.ReadByte();
			Int32 b2 = Stream.ReadByte();
			Int32 b1 = Stream.ReadByte();
			Int32 b0 = Stream.ReadByte();
			if (b3 == -1 || b2 == -1 || b1 == -1 || b0 == -1) {
				return -1;
			}
			b2 += b3 << 8;
			b1 += b2 << 8;
			b0 += b1 << 8;
			return b0;
		}
	}
}