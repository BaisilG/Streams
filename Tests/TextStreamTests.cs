using System;
using System.IO;
using Stringier.Streams;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class TextStreamTests {
		[Fact]
		public void Constructor() => new TextStream<StringStream>(new StringStream("hello world!"));

		[Fact]
		public void ReadSeek_StringStream() {
			using Stream stream = new TextStream<StringStream>(new StringStream("hello world"));
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
			stream.Position = 0;
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
		}
	}
}
