using System;
using Stringier.Encodings;

namespace Stringier.Streams {

	internal sealed class Utf8 : EncodingHelper {
		/// <summary>
		/// The second char of a multi-char sequence.
		/// </summary>
		private Int32? secondChar;

		/// <inheritdoc/>
		public static Byte[] BOM => new Byte[] { 0xEF, 0xBB, 0xBF };

		/// <inheritdoc/>
		public override Encoding Enum => Encoding.UTF8;

		/// <inheritdoc/>
		public override Int32 ReadChar() {
			if (secondChar.HasValue) {
				Int32 @char = secondChar.Value;
				secondChar = null;
				return @char;
			}
			Int32 first = Stream.ReadByte();
			Int32 second;
			Int32 third;
			Int32 fourth;
			switch (Encodings.Utf8.SequenceLength((Byte)first)) {
			case 1:
				return first;
			case 2:
				second = Stream.ReadByte();
				if (second == -1) {
					return -1;
				}
				return Encodings.Utf8.Decode((Byte)first, (Byte)second).Value;
			case 3:
				second = Stream.ReadByte();
				third = Stream.ReadByte();
				if (second == -1 || third == -1) {
					return -1;
				}
				return Encodings.Utf8.Decode((Byte)first, (Byte)second, (Byte)third).Value;
			case 4:
				second = Stream.ReadByte();
				third = Stream.ReadByte();
				fourth = Stream.ReadByte();
				if (second == -1 || third == -1 || fourth == -1) {
					return -1;
				}
				Char[] chars = Utf16.Encode(Encodings.Utf8.Decode((Byte)first, (Byte)second, (Byte)third, (Byte)fourth))!;
				secondChar = chars[1];
				return chars[0];
			default:
				return -1;
			}
		}

		/// <inheritdoc/>
		public override Int32 ReadRune() {
			if (secondChar.HasValue) {
				return -1;
			}
			Int32 first = Stream.ReadByte();
			Int32 second;
			Int32 third;
			Int32 fourth;
			switch (Encodings.Utf8.SequenceLength((Byte)first)) {
			case 1:
				return first;
			case 2:
				second = Stream.ReadByte();
				if (second == -1) {
					return -1;
				}
				return Encodings.Utf8.Decode((Byte)first, (Byte)second).Value;
			case 3:
				second = Stream.ReadByte();
				third = Stream.ReadByte();
				if (second == -1 || third == -1) {
					return -1;
				}
				return Encodings.Utf8.Decode((Byte)first, (Byte)second, (Byte)third).Value;
			case 4:
				second = Stream.ReadByte();
				third = Stream.ReadByte();
				fourth = Stream.ReadByte();
				if (second == -1 || third == -1 || fourth == -1) {
					return -1;
				}
				return Encodings.Utf8.Decode((Byte)first, (Byte)second, (Byte)third, (Byte)fourth).Value;
			default:
				return -1;
			}
		}
	}
}