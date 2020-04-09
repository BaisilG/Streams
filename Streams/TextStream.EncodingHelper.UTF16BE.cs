using System;
using Stringier.Encodings;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		internal abstract partial class EncodingHelper {
			/// <summary>
			/// UTF-16 big endian
			/// </summary>
			public static EncodingHelper UTF16BE { get; } = new UTF16BEEncodingHelper();

			internal sealed class UTF16BEEncodingHelper : EncodingHelper {
				public override Byte[] BOM => new Byte[] { 0xFE, 0xFF };

				/// <inheritdoc/>
				public override Encoding Enum => Encoding.UTF16BE;

				/// <inheritdoc/>
				public override Int32 ReadChar(TextStream stream) {
					Int32 big = stream.ReadByte();
					Int32 little = stream.ReadByte();
					if (big == -1 || little == -1) {
						return -1;
					}
					little += big << 8;
					return little;
				}

				/// <inheritdoc/>
				public override Int32 ReadRune(TextStream stream) {
					Int32 high = ReadChar(stream);
					if (high == -1) {
						return -1;
					}
					switch (Utf16.SequenceLength((UInt16)high)) {
					case 1:
						return high;
					case 2:
						return Utf16.Decode((UInt16)high, (UInt16)ReadChar(stream)).Value;
					default:
						return -1;
					}
				}
			}
		}
	}
}