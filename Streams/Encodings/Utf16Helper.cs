using System;
using System.Text;
using Stringier.Encodings;

namespace Stringier.Streams {
	internal abstract class Utf16Helper : EncodingHelper {
		/// <inheritdoc/>
		public sealed override Int32 ReadRune() {
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

		/// <inheritdoc/>
		public sealed override void WriteRune(Rune value) {
			foreach (Char @char in Utf16.Encode(value)!) {
				WriteChar(@char);
			}
		}
	}
}
