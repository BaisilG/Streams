using System;
using System.Collections.Generic;
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
				/// The <see cref="Encoding"/> enum representing this helper.
				/// </summary>
				public override Encoding Enum => Encoding.UTF8;

				public UTF8EncodingHelper() : base(bom: new Byte[] { 0xEF, 0xBB, 0xBF }) {

				}
			}
		}
	}
}