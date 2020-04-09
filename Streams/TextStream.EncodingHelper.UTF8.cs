using System;
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
				/// <summary>
				/// The second char of a multi-char sequence.
				/// </summary>
				private Int32? secondChar;

				/// <inheritdoc/>
				public override Byte[] BOM => new Byte[] { 0xEF, 0xBB, 0xBF };

				/// <inheritdoc/>
				public override Encoding Enum => Encoding.UTF8;

				/// <inheritdoc/>
				public override Int32 ReadChar(TextStream stream) {
					if (secondChar.HasValue) {
						Int32 @char = secondChar.Value;
						secondChar = null;
						return @char;
					}
					Int32 first = stream.ReadByte();
					Int32 second;
					Int32 third;
					Int32 fourth;
					switch (Utf8.SequenceLength((Byte)first)) {
					case 1:
						return first;
					case 2:
						second = stream.ReadByte();
						if (second == -1) {
							return -1;
						}
						return Utf8.Decode((Byte)first, (Byte)second).Value;
					case 3:
						second = stream.ReadByte();
						third = stream.ReadByte();
						if (second == -1 || third == -1) {
							return -1;
						}
						return Utf8.Decode((Byte)first, (Byte)second, (Byte)third).Value;
					case 4:
						second = stream.ReadByte();
						third = stream.ReadByte();
						fourth = stream.ReadByte();
						if (second == -1 || third == -1 || fourth == -1) {
							return -1;
						}
						Char[] chars = Utf16.Encode(Utf8.Decode((Byte)first, (Byte)second, (Byte)third, (Byte)fourth))!;
						secondChar = chars[1];
						return chars[0];
					default:
						return -1;
					}
				}

				/// <inheritdoc/>
				public override Int32 ReadRune(TextStream stream) {
					if (secondChar.HasValue) {
						return -1;
					}
					Int32 first = stream.ReadByte();
					Int32 second;
					Int32 third;
					Int32 fourth;
					switch (Utf8.SequenceLength((Byte)first)) {
					case 1:
						return first;
					case 2:
						second = stream.ReadByte();
						if (second == -1) {
							return -1;
						}
						return Utf8.Decode((Byte)first, (Byte)second).Value;
					case 3:
						second = stream.ReadByte();
						third = stream.ReadByte();
						if (second == -1 || third == -1) {
							return -1;
						}
						return Utf8.Decode((Byte)first, (Byte)second, (Byte)third).Value;
					case 4:
						second = stream.ReadByte();
						third = stream.ReadByte();
						fourth = stream.ReadByte();
						if (second == -1 || third == -1 || fourth == -1) {
							return -1;
						}
						return Utf8.Decode((Byte)first, (Byte)second, (Byte)third, (Byte)fourth).Value;
					default:
						return -1;
					}
				}
			}
		}
	}
}