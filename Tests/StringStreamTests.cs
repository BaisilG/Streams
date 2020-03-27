using System;
using System.IO;
using Stringier.Streams;
using Defender;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class StringStreamTests : Trial {
		[Fact]
		public void Constructor() => new StringStream("hello");

		[Theory]
		[InlineData("hello")]
		[InlineData("привет")]
		[InlineData("こんにちは")]
		public void Reader(String source) {
			using Stream stream = new StringStream(source);
			using StreamReader reader = new StreamReader(stream, System.Text.Encoding.Unicode);
			Assert.Equal(source, reader.ReadToEnd());
		}
    }
}
