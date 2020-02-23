using System;
using System.IO;
using Stringier.Streams;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class TextReaderExtensionsTests {
		[Fact]
		public void Read_StringStream() {
			using Stream stream = new StringStream("helloworld!");
			using TextReader reader = new StreamReader(stream);
			Assert.Equal("hello", reader.Read(5));
			Assert.Equal("world", reader.Read(5));
			Assert.Equal("!", reader.Read(5));
		}

		[Fact]
		public void Read_FileStream() {
			using Stream stream = new FileStream("Test.txt", FileMode.Open);
			using TextReader reader = new StreamReader(stream);
			Assert.Equal("hello", reader.Read(5));
			Assert.Equal("world", reader.Read(5));
			Assert.Equal("!", reader.Read(5));
		}
	}
}
