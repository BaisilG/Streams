using System;
using System.Text;
using Stringier.Encodings;

namespace Stringier.Streams {
	internal abstract class Utf32Helper : EncodingHelper {
		/// <summary>
		/// The second char of a multi-char sequence.
		/// </summary>
		private Int32? secondChar;

		/// <inheritdoc/>
		public sealed override Int32 ReadChar() {
			if (secondChar.HasValue) {
				Int32 @char = secondChar.Value;
				secondChar = null;
				return @char;
			}
			Int32 rune = ReadRune();
			if (rune < 0) {
				return rune;
			}
			Char[] chars = Utf16.Encode(new Rune(rune))!;
			switch (Utf16.SequenceLength(chars[0])) {
			case 1:
				return rune;
			case 2:
				secondChar = chars[1];
				return chars[0];
			default:
				return -1;
			}
		}
	}
}
