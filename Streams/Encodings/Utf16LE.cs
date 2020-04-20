using System;
using Stringier.Encodings;

namespace Stringier.Streams {
	internal sealed class Utf16LE : EncodingHelper {
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
