using System;
using System.IO;
using System.Text;
using Stringier.Streams;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class StreamExtensionsTests {
		[Fact]
		public void ReadByte_FileStream() {
			using Stream stream = new FileStream("Test.txt", FileMode.Open);
			Assert.Equal(new Byte[] { 0xEF, 0xBB, 0xBF }, stream.Read(3));
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
		}

		[Fact]
		public void ReadByte_StringStream() {
			using Stream stream = new StringStream("helloworld");
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
		}

		[Fact]
		public void ReadSeekByte_FileStream() {
			using Stream stream = new FileStream("Test.txt", FileMode.Open);
			Assert.Equal(new Byte[] { 0xEF, 0xBB, 0xBF }, stream.Read(3));
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
			stream.Position = 0;
			Assert.Equal(new Byte[] { 0xEF, 0xBB, 0xBF }, stream.Read(3));
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
		}

		[Fact]
		public void ReadSeekByte_StringStream() {
			using Stream stream = new StringStream("helloworld");
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
			stream.Position = 0;
			Assert.Equal(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }, stream.Read(5));
		}

		[Fact]
		public void ReadRune_StringStream() {
			using Stream stream = new StringStream("AöЖ€𝄞");
			Assert.Equal(new Rune('A'), stream.ReadRune());
			Assert.Equal(new Rune('ö'), stream.ReadRune());
			Assert.Equal(new Rune('Ж'), stream.ReadRune());
			Assert.Equal(new Rune('€'), stream.ReadRune());
			Assert.Equal(new Rune(0x1D11E), stream.ReadRune());
			stream.Position = 0;
			Assert.Equal(new[] { new Rune('A'), new Rune('ö'), new Rune('Ж') }, stream.ReadRune(3));
			Assert.Equal(new[] { new Rune('€'), new Rune(0x1D11E) }, stream.ReadRune(2));
		}
	}
}
