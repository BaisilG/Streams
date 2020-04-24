using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Defender;
using Stringier.Streams.Buffers;
using Buffer = Stringier.Streams.Buffers.Buffer;

namespace Stringier.Streams {
	/// <summary>
	/// Provides a view of a sequence of characters.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type uses a very minor buffering approach to enable peeking. Unlike <see cref="TextReader"/> and <see cref="TextWriter"/>, the stream is fully aware of the buffer, and discards it whenever appropriate. This means unlike <see cref="TextReader"/> and <see cref="TextWriter"/>, using <see cref="Seek(Int64, SeekOrigin)"/> or <see cref="Position"/> isn't broken.
	/// </para>
	/// <para>
	/// The buffer is typically never used, especially during normal <see cref="Stream"/> operations. Certain <see cref="PeekByte"/> and related methods do use it. Because the <see cref="TextStream"/> is aware of its buffer, all related methods "do the right thing" with regards to it. Attempts are made to flush the buffer and keep it flushed as much as possible.
	/// </para>
	/// <para>
	/// There are also some special <see cref="TextStream"/> methods which use the buffer for reading, such as <see cref="ReadRune"/> and related methods, which read more than a single byte.
	/// </para>
	/// </remarks>
	public sealed class TextStream : Stream {
		/// <summary>
		/// The underlying <see cref="Stream"/>.
		/// </summary>
		private readonly Stream BaseStream;

		/// <summary>
		/// The read buffer.
		/// </summary>
		private readonly IReadBuffer ReadBuffer;

		/// <summary>
		/// The write buffer.
		/// </summary>
		private readonly IWriteBuffer WriteBuffer;

		/// <summary>
		/// The encoding helper for this <see cref="TextStream"/>.
		/// </summary>
		private readonly EncodingHelper Helper;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStream"/> class.
		/// </summary>
		/// <param name="stream">The underlying <see cref="Stream"/>.</param>
		/// <remarks>
		/// This attempts to determine the encoding automatically, defaulting to <see cref="Encoding.UTF8"/>. It will consume the BOM.
		/// </remarks>
		public TextStream(Stream stream) : this(stream, null, null) { }

		/// <summary>
		/// Initialize a new instance of the <see cref="TextStream"/> class.
		/// </summary>
		/// <param name="stream">The underlying <see cref="Stream"/>.</param>
		/// <param name="readBuffer">The read buffer.</param>
		/// <param name="writeBuffer">The write buffer.</param>
		/// <remarks>
		/// This attempts to determine the encoding only by looking at the BOM. While this is fast and accurate in most cases, it can fail; so can the alternatives. Be aware of this possibility. In cases where rereading is not generally possible, such as network streams, it is strongly advized to use a consistent encoding or utilize a handshake procedure to establish the encoding explicitly.
		/// </remarks>
		public TextStream(Stream stream, Buffer? readBuffer, Buffer? writeBuffer) {
			BaseStream = stream;
			ReadBuffer = readBuffer ?? new PassthroughBuffer();
			ReadBuffer.Stream = BaseStream;
			WriteBuffer = writeBuffer ?? new PassthroughBuffer();
			WriteBuffer.Stream = BaseStream;
			ReadBuffer.Read(4);
			if (ReadBuffer.Equals(Utf8Helper.BOM)) {
				ReadBuffer.Shift(Utf8Helper.BOM.Length);
				Helper = new Utf8Helper();
			} else if (ReadBuffer.Equals(Utf32LEHelper.BOM)) { // This must be checked before UTF-16LE, even though it's very unlikely
				ReadBuffer.Shift(Utf32LEHelper.BOM.Length);
				Helper = new Utf32LEHelper();
			} else if (ReadBuffer.Equals(Utf16LEHelper.BOM)) {
				ReadBuffer.Shift(Utf16LEHelper.BOM.Length);
				Helper = new Utf16LEHelper();
			} else if (ReadBuffer.Equals(Utf16BEHelper.BOM)) {
				ReadBuffer.Shift(Utf16BEHelper.BOM.Length);
				Helper = new Utf16BEHelper();
			} else if (ReadBuffer.Equals(Utf32BEHelper.BOM)) {
				ReadBuffer.Shift(Utf32BEHelper.BOM.Length);
				Helper = new Utf32BEHelper();
			} else {
				// There wasn't a BOM, so use the default.
				Helper = new Utf8Helper();
			}
			Helper.Stream = this;
		}

		/// <inheritdoc/>
		public override Boolean CanRead => ReadBuffer.CanRead || BaseStream.CanRead;

		/// <inheritdoc/>
		public override Boolean CanSeek => ReadBuffer.CanSeek || BaseStream.CanSeek;

		/// <inheritdoc/>
		public override Boolean CanTimeout => BaseStream.CanTimeout;

		/// <inheritdoc/>
		public override Boolean CanWrite => WriteBuffer.CanWrite || BaseStream.CanWrite;

		/// <summary>
		/// The encoding of a <see cref="TextStream"/>.
		/// </summary>
		public Encoding Encoding => Helper.Enum;

		/// <inheritdoc/>
		public override Int64 Length => BaseStream.Length;

		/// <inheritdoc/>
		public override Int64 Position {
			get => ReadBuffer.Stale ? BaseStream.Position : BaseStream.Position - ReadBuffer.Length + ReadBuffer.Position;
			set => BaseStream.Position = value;
		}

		/// <inheritdoc/>
		public override Int32 ReadTimeout {
			get => BaseStream.ReadTimeout;
			set => BaseStream.ReadTimeout = value;
		}

		/// <inheritdoc/>
		public override Int32 WriteTimeout {
			get => BaseStream.WriteTimeout;
			set => BaseStream.WriteTimeout = value;
		}

		/// <inheritdoc/>
		public override IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object? state) => BaseStream.BeginRead(buffer, offset, count, callback, state);

		/// <inheritdoc/>
		public override IAsyncResult BeginWrite(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object? state) => BaseStream.BeginWrite(buffer, offset, count, callback, state);

		/// <inheritdoc/>
		public override void Close() => BaseStream.Close();

		/// <inheritdoc/>
		public override void CopyTo(Stream destination, Int32 bufferSize) => BaseStream.CopyTo(destination, bufferSize);

		/// <inheritdoc/>
		public override Task CopyToAsync(Stream destination, Int32 bufferSize, CancellationToken cancellationToken) {
			Guard.NotNull(destination, nameof(destination));
			return BaseStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		/// <inheritdoc/>
		public override ValueTask DisposeAsync() => BaseStream.DisposeAsync();

		/// <inheritdoc/>
		public override Int32 EndRead(IAsyncResult asyncResult) {
			Guard.NotNull(asyncResult, nameof(asyncResult));
			return BaseStream.EndRead(asyncResult);
		}

		/// <inheritdoc/>
		public override void EndWrite(IAsyncResult asyncResult) {
			Guard.NotNull(asyncResult, nameof(asyncResult));
			BaseStream.EndWrite(asyncResult);
		}

		/// <inheritdoc/>
		public override Boolean Equals(Object? obj) => BaseStream.Equals(obj);

		/// <inheritdoc/>
		public override void Flush() => BaseStream.Flush();

		/// <inheritdoc/>
		public override Task FlushAsync(CancellationToken cancellationToken) => BaseStream.FlushAsync(cancellationToken);

		/// <inheritdoc/>
		public override Int32 GetHashCode() => BaseStream.GetHashCode();

		/// <inheritdoc/>
		public override Object InitializeLifetimeService() => BaseStream.InitializeLifetimeService();

		/// <summary>
		/// Peeks at a byte from the stream but does not advance the position within the stream. Returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>The unsigned byte cast to an <see cref="Int32"/>, or -1 if at the end of the stream.</returns>
		public Int32 PeekByte() => ReadBuffer.Peek();

		/// <inheritdoc/>
		public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count) => Read(buffer.AsSpan(offset, count));

		/// <inheritdoc/>
		public override Int32 Read(Span<Byte> buffer) => ReadBuffer.Get(buffer);

		/// <inheritdoc/>
		public override ValueTask<Int32> ReadAsync(Memory<Byte> buffer, CancellationToken cancellationToken = default) => BaseStream.ReadAsync(buffer, cancellationToken);

		/// <inheritdoc/>
		public override Task<Int32> ReadAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken) => BaseStream.ReadAsync(buffer, offset, count, cancellationToken);

		/// <inheritdoc/>
		public override Int32 ReadByte() => ReadBuffer.Get();

		/// <summary>
		/// Reads a char from the stream and advances the position within the stream by the encoding byte count, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>The unsigned char cast to an <see cref="Int32"/>, or -1 if at the end of the stream.</returns>
		public Int32 ReadChar() => Helper.ReadChar();

		/// <summary>
		/// Reads a rune from the stream and advances the position within the stream by the encoding byte count, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>The unsigned rune cast to an <see cref="Int32"/>, or -1 if at the end of the stream.</returns>
		public Int32 ReadRune() => Helper.ReadRune();

		/// <inheritdoc/>
		public override Int64 Seek(Int64 offset, SeekOrigin origin) {
			switch (origin) {
			case SeekOrigin.Current:
				if (ReadBuffer.Stale) {
					goto default;
				} else {
					return BaseStream.Seek(offset + ReadBuffer.Length, origin);
				}
			default:
				return BaseStream.Seek(offset, origin);
			}
		}

		/// <inheritdoc/>
		public override void SetLength(Int64 value) => BaseStream.SetLength(value);

		/// <inheritdoc/>
		public override String ToString() => BaseStream.ToString();

		/// <inheritdoc/>
		public override void Write(ReadOnlySpan<Byte> buffer) => BaseStream.Write(buffer);

		/// <inheritdoc/>
		public override void Write(Byte[] buffer, Int32 offset, Int32 count) => BaseStream.Write(buffer, offset, count);

		/// <inheritdoc/>
		public override ValueTask WriteAsync(ReadOnlyMemory<Byte> buffer, CancellationToken cancellationToken = default) => BaseStream.WriteAsync(buffer, cancellationToken);

		/// <inheritdoc/>
		public override Task WriteAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken) => BaseStream.WriteAsync(buffer, offset, count, cancellationToken);

		/// <inheritdoc/>
		public override void WriteByte(Byte value) => BaseStream.WriteByte(value);
	}
}
