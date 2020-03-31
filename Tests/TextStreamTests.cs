using System;
using System.IO;
using Stringier.Streams;
using Defender;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class TextStreamTests : Trial {
		[Fact]
		public void Constructor() {
			var stream = new TextStream(new StringStream("hello world!"));
			Assert.Equal(Encoding.UTF8, stream.Encoding);
			stream.Dispose();
			stream = new TextStream(new MemoryStream(new Byte[] { 0xEF, 0xBB, 0xBF, 0x68, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x77, 0x6F, 0x72, 0x6C, 0x64 }));
			Assert.Equal(Encoding.UTF8, stream.Encoding);
			stream.Dispose();
			stream = new TextStream(new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00, 0x20, 0x00, 0x77, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6C, 0x00, 0x64, 0x00 }));
			Assert.Equal(Encoding.UTF16LE, stream.Encoding);
			stream.Dispose();
			stream = new TextStream(new MemoryStream(new Byte[] { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00, 0x20, 0x00, 0x77, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6C, 0x00, 0x64 }));
			Assert.Equal(Encoding.UTF16BE, stream.Encoding);
			stream.Dispose();
		}

		[Fact]
		public void CanRead() {
			using StringStream inner = new StringStream("hello world");
			using TextStream stream = new TextStream(inner);
			Assert.Equal(inner.CanRead, stream.CanRead);
		}

		[Fact]
		public void CanSeek() {
			using StringStream inner = new StringStream("hello world");
			using TextStream stream = new TextStream(inner);
			Assert.Equal(inner.CanSeek, stream.CanSeek);
		}

		[Fact]
		public void CanWrite() {
			using StringStream inner = new StringStream("hello world");
			using TextStream stream = new TextStream(inner);
			Assert.Equal(inner.CanWrite, stream.CanWrite);
		}

		[Fact]
		public void PeekByte() {
			using TextStream stream = new TextStream(new StringStream("hello world"));
			Assert.Equal(0x68, stream.PeekByte());
			Assert.Equal(0x68, stream.PeekByte());
			Assert.Equal(0x68, stream.ReadByte());
			Assert.Equal(0x00, stream.ReadByte());
			Assert.Equal(0x65, stream.ReadByte());
			Assert.Equal(0x00, stream.PeekByte());
			Assert.Equal(0x00, stream.ReadByte());
			Assert.Equal(0x6C, stream.PeekByte());
			Assert.Equal(0x6C, stream.ReadByte());
		}

		[Fact]
		public void Position() {
			using TextStream stream = new TextStream(new StringStream("hello world"));
			Assert.Equal(0, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(1, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(2, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(3, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(4, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(5, stream.Position);
			_ = stream.Seek(0, SeekOrigin.Begin);
			Assert.Equal(0, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(1, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(2, stream.Position);
			_ = stream.PeekByte();
			Assert.Equal(2, stream.Position);
			_ = stream.PeekByte();
			Assert.Equal(2, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(3, stream.Position);
			_ = stream.PeekByte();
			Assert.Equal(3, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(4, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(5, stream.Position);
		}

		[Fact]
		public void Read() {
			using TextStream stream = new TextStream(new StringStream("hello world, how are you today?"));
			Byte[] read = new Byte[8];
			Assert.Equal(0, stream.Position);
			// Remember, we're starting off with 4 bytes already buffered, this will read part of that buffer.
			Assert.Equal(2, stream.Read(read, 0, 2));
			Assert.Equal(2, stream.Position);
			Assert.Equal(0x68, read[0]);
			Assert.Equal(0x00, read[1]);
			// And then it will read the remaining part of the buffer, and the rest of what's needed from the stream.
			Assert.Equal(8, stream.Read(read, 0, 8));
			Assert.Equal(10, stream.Position);
			Assert.Equal(0x65, read[0]);
			Assert.Equal(0x00, read[1]);
			Assert.Equal(0x6C, read[2]);
			Assert.Equal(0x00, read[3]);
			Assert.Equal(0x6C, read[4]);
			Assert.Equal(0x00, read[5]);
			Assert.Equal(0x6F, read[6]);
			Assert.Equal(0x00, read[7]);
			// The buffer has been completely flushed by this point, so now reads will be directly from the stream
			Assert.Equal(2, stream.Read(read, 0, 2));
			Assert.Equal(12, stream.Position);
			Assert.Equal(0x20, read[0]);
			Assert.Equal(0x00, read[1]);
		}
	}
}
