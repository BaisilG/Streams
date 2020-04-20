using System;
using Stringier.Encodings;

namespace Stringier.Streams {
	internal sealed class Utf16BE : EncodingHelper {
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
		public override Int32 ReadRune() {
			Int32 high = ReadChar();
			if (high == -1) {
				return -1;
			}
			switch (Utf16.SequenceLength((UInt16)high)) {
			case 1:
				return high;
			case 2:
				return Utf16.Decode((UInt16)high, (UInt16)ReadChar()).Value;
			default:
				return -1;
			}
		}
	}
}