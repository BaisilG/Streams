using System;
using System.IO;
using Stringier.Streams;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class StringStreamTests {
		[Fact]
		public void Constructor() => new StringStream("hello");

		[Theory]
		[InlineData("hello")]
		[InlineData("привет")]
		[InlineData("こんにちは")]
		public void Reader(String source) {
			using Stream stream = new StringStream(source);
			using StreamReader reader = new StreamReader(stream);

			Assert.Equal(source, reader.ReadToEnd());
		}

		[Fact]
		public void Seek() {
			using Stream stream = new StringStream("hello");
			using StreamReader reader = new StreamReader(stream);
			_ = stream.Seek(3L, SeekOrigin.Begin);
			Assert.Equal("lo", reader.ReadToEnd());
			_ = stream.Seek(0L, SeekOrigin.Begin);
			Assert.Equal("hello", reader.ReadToEnd());
		}
    }
}
