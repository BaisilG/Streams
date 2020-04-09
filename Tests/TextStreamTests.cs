using System;
using System.IO;
using Stringier.Streams;
using Defender;
using Xunit;

namespace Tests {
	[Collection("Tests")]
	public class TextStreamTests : Trial {
		[Theory]
		[ClassData(typeof(TextStreamConstructorData))]
		public void Constructor(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Encoding encoding) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(encoding, stream.Encoding);
		}

		[Theory]
		[ClassData(typeof(CanReadData))]
		public void CanRead(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Boolean expected) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(expected, stream.CanRead);
		}

		[Theory]
		[ClassData(typeof(CanSeekData))]
		public void CanSeek(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Boolean expected) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(expected, stream.CanSeek);
		}

		[Theory]
		[ClassData(typeof(CanWriteData))]
		public void CanWrite(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Boolean expected) {
			using TextStream stream = writeBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(expected, stream.CanWrite);
		}

		[Theory]
		[ClassData(typeof(PeekByteData))]
		public void PeekByte(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Int32 p1, Int32 p2, Int32 p3, Int32 p4, Int32 p5, Int32 p6, Int32 p7, Int32 p8, Int32 p9) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(p1, stream.PeekByte());
			Assert.Equal(p2, stream.PeekByte());
			Assert.Equal(p3, stream.ReadByte());
			Assert.Equal(p4, stream.ReadByte());
			Assert.Equal(p5, stream.ReadByte());
			Assert.Equal(p6, stream.PeekByte());
			Assert.Equal(p7, stream.ReadByte());
			Assert.Equal(p8, stream.PeekByte());
			Assert.Equal(p9, stream.ReadByte());
		}

		[Theory]
		[ClassData(typeof(PositionData))]
		public void Position(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Int32 p1, Int32 p2, Int32 p3, Int32 p4, Int32 p5, Int32 p6, Int32 p7, Int32 p8, Int32 p9, Int32 p10, Int32 p11, Int32 p12, Int32 p13, Int32 p14, Int32 p15) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(p1, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p2, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p3, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p4, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p5, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p6, stream.Position);
			_ = stream.Seek(0, SeekOrigin.Begin);
			Assert.Equal(p7, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p8, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p9, stream.Position);
			_ = stream.PeekByte();
			Assert.Equal(p10, stream.Position);
			_ = stream.PeekByte();
			Assert.Equal(p11, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p12, stream.Position);
			_ = stream.PeekByte();
			Assert.Equal(p13, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p14, stream.Position);
			_ = stream.ReadByte();
			Assert.Equal(p15, stream.Position);
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

		[Theory]
		[ClassData(typeof(ReadCharData))]
		public void ReadChar(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Int32 p1, Int32 c1, Int32 p2, Int32 c2, Int32 p3, Int32 c3, Int32 p4, Int32 c4, Int32 p5, Int32 c5) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(p1, stream.Position);
			Assert.Equal(c1, stream.ReadChar());
			Assert.Equal(p2, stream.Position);
			Assert.Equal(c2, stream.ReadChar());
			Assert.Equal(p3, stream.Position);
			Assert.Equal(c3, stream.ReadChar());
			Assert.Equal(p4, stream.Position);
			Assert.Equal(c4, stream.ReadChar());
			Assert.Equal(p5, stream.Position);
			Assert.Equal(c5, stream.ReadChar());
		}

		[Theory]
		[ClassData(typeof(ReadRuneData))]
		public void ReadRune(Stream baseStream, IReadBuffer? readBuffer, IWriteBuffer? writeBuffer, Int32 p1, Int32 c1, Int32 p2, Int32 c2, Int32 p3, Int32 c3, Int32 p4, Int32 c4, Int32 p5, Int32 c5) {
			using TextStream stream = readBuffer is null ? new TextStream(baseStream) : new TextStream(baseStream, readBuffer, writeBuffer);
			Assert.Equal(p1, stream.Position);
			Assert.Equal(c1, stream.ReadRune());
			Assert.Equal(p2, stream.Position);
			Assert.Equal(c2, stream.ReadRune());
			Assert.Equal(p3, stream.Position);
			Assert.Equal(c3, stream.ReadRune());
			Assert.Equal(p4, stream.Position);
			Assert.Equal(c4, stream.ReadRune());
			Assert.Equal(p5, stream.Position);
			Assert.Equal(c5, stream.ReadRune());
		}
	}
}
