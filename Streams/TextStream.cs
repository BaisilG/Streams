﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Defender;
using static Stringier.Streams.TextStream.EncodingHelper;

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
	public sealed partial class TextStream : Stream {
		/// <summary>
		/// The underlying <see cref="Stream"/>.
		/// </summary>
		private readonly Stream baseStream;

		/// <summary>
		/// A byte buffer used to enable peeking.
		/// </summary>
		/// <remarks>
		/// <para>
		/// In most cases this won't even be used, and when it is will be a single, specific, byte. BOM determination requires multiple bytes, however, so in those cases it will be a sequence.
		/// </para>
		/// <para>
		/// This is stored as a <see cref="Int32"/> to comply with C/C++/C# stream conventions, which use -1 for errors, and to simplify the code in which <see cref="buffer"/> is used.
		/// </para>
		/// </remarks>
		private readonly Buffer buffer = new Buffer();

		/// <summary>
		/// The encoding helper for this <see cref="TextStream"/>.
		/// </summary>
		private readonly EncodingHelper helper;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStream"/> class.
		/// </summary>
		/// <param name="stream">The underlying <see cref="Stream"/>.</param>
		/// <remarks>
		/// This attempts to determine the encoding automatically, defaulting to <see cref="Encoding.UTF8"/>. It will consume the BOM.
		/// </remarks>
		public TextStream(Stream stream) {
			baseStream = stream;
			FillBuffer();
			if (buffer == UTF8.BOM) {
				buffer <<= UTF8.BOM.Length;
				helper = UTF8;
			} else if (buffer == UTF32LE.BOM) { // This must be checked before UTF-16LE, even though it's very unlikely
				buffer <<= UTF32LE.BOM.Length;
				helper = UTF32LE;
			} else if (buffer == UTF16LE.BOM) {
				buffer <<= UTF16LE.BOM.Length;
				helper = UTF16LE;
			} else if (buffer == UTF16BE.BOM) {
				buffer <<= UTF16BE.BOM.Length;
				helper = UTF16BE;
			} else if (buffer == UTF32BE.BOM) {
				buffer <<= UTF32BE.BOM.Length;
				helper = UTF32BE;
			} else {
				// There wasn't a BOM, so use the default.
				helper = UTF8;
			}
		}

		/// <inheritdoc/>
		public override Boolean CanRead => baseStream.CanRead;

		/// <inheritdoc/>
		public override Boolean CanSeek => baseStream.CanSeek;

		/// <inheritdoc/>
		public override Boolean CanTimeout => baseStream.CanTimeout;

		/// <inheritdoc/>
		public override Boolean CanWrite => baseStream.CanWrite;

		/// <summary>
		/// The encoding of a <see cref="TextStream"/>.
		/// </summary>
		public Encoding Encoding => helper.Enum;

		/// <inheritdoc/>
		public override Int64 Length => baseStream.Length;

		/// <inheritdoc/>
		public override Int64 Position {
			get => buffer.Stale ? baseStream.Position : baseStream.Position - buffer.Length;
			set => baseStream.Position = value;
		}

		/// <inheritdoc/>
		public override Int32 ReadTimeout {
			get => baseStream.ReadTimeout;
			set => baseStream.ReadTimeout = value;
		}

		/// <inheritdoc/>
		public override Int32 WriteTimeout {
			get => baseStream.WriteTimeout;
			set => baseStream.WriteTimeout = value;
		}

		/// <inheritdoc/>
		public override IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object? state) => baseStream.BeginRead(buffer, offset, count, callback, state);

		/// <inheritdoc/>
		public override IAsyncResult BeginWrite(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object? state) => baseStream.BeginWrite(buffer, offset, count, callback, state);

		/// <inheritdoc/>
		public override void Close() => baseStream.Close();

		/// <inheritdoc/>
		public override void CopyTo(Stream destination, Int32 bufferSize) => baseStream.CopyTo(destination, bufferSize);

		/// <inheritdoc/>
		public override Task CopyToAsync(Stream destination, Int32 bufferSize, CancellationToken cancellationToken) {
			Guard.NotNull(destination, nameof(destination));
			return baseStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		/// <inheritdoc/>
		public override ValueTask DisposeAsync() => baseStream.DisposeAsync();

		/// <inheritdoc/>
		public override Int32 EndRead(IAsyncResult asyncResult) {
			Guard.NotNull(asyncResult, nameof(asyncResult));
			return baseStream.EndRead(asyncResult);
		}

		/// <inheritdoc/>
		public override void EndWrite(IAsyncResult asyncResult) {
			Guard.NotNull(asyncResult, nameof(asyncResult));
			baseStream.EndWrite(asyncResult);
		}

		/// <inheritdoc/>
		public override Boolean Equals(Object? obj) => baseStream.Equals(obj);

		/// <inheritdoc/>
		public override void Flush() => baseStream.Flush();

		/// <inheritdoc/>
		public override Task FlushAsync(CancellationToken cancellationToken) => baseStream.FlushAsync(cancellationToken);

		/// <inheritdoc/>
		public override Int32 GetHashCode() => baseStream.GetHashCode();

		/// <inheritdoc/>
		public override Object InitializeLifetimeService() => baseStream.InitializeLifetimeService();

		/// <summary>
		/// Peeks at a byte from the stream but does not advance the position within the stream. Returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>The unsigned byte cast to an <see cref="Int32"/>, or -1 if at the end of the stream.</returns>
		public Int32 PeekByte() {
			if (buffer.Stale) {
				buffer.Set(baseStream.ReadByte());
			}
			return buffer.Peek();
		}

		/// <inheritdoc/>
		public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count) => Read(buffer.AsSpan(offset, count));

		/// <inheritdoc/>
		public override Int32 Read(Span<Byte> buffer) {
			if (this.buffer.Stale) {
				return baseStream.Read(buffer);
			}
			if (this.buffer.Length < buffer.Length) {
				//This scenario sucks. We have to read partially from the buffer, and the remaining amount from the stream
				Int32 val;
				Int32 i = 0;
				Int32 r = buffer.Length;
				// Read from the buffer
				while (i < this.buffer.Length && r-- > 0) {
					val = this.buffer.Get();
					buffer[i++] = val == -1 ? (Byte)0x00 : (Byte)val;
				}
				// Read from the stream
				while (i < buffer.Length && r-- > 0) {
					val = ReadByte();
					buffer[i++] = val == -1 ? (Byte)0x00 : (Byte)val;
				}
				return i;
			} else {
				//This scenario isn't too bad. The buffer is the same size or larger than what we want to read, so read that part of the buffer.
				Int32 first, second, third, fourth;
				switch (buffer.Length) {
				case 1:
					this.buffer.Get(out first);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					return 1;
				case 2:
					this.buffer.Get(out first, out second);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					buffer[1] = second == -1 ? (Byte)0x00 : (Byte)second;
					return 2;
				case 3:
					this.buffer.Get(out first, out second, out third);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					buffer[1] = second == -1 ? (Byte)0x00 : (Byte)second;
					buffer[2] = third == -1 ? (Byte)0x00 : (Byte)third;
					return 3;
				case 4:
					this.buffer.Get(out first, out second, out third, out fourth);
					buffer[0] = first == -1 ? (Byte)0x00 : (Byte)first;
					buffer[1] = second == -1 ? (Byte)0x00 : (Byte)second;
					buffer[2] = third == -1 ? (Byte)0x00 : (Byte)third;
					buffer[3] = fourth == -1 ? (Byte)0x00 : (Byte)fourth;
					return 4;
				default:
					return 0;
				}
			}
		}

		/// <inheritdoc/>
		public override ValueTask<Int32> ReadAsync(Memory<Byte> buffer, CancellationToken cancellationToken = default) => baseStream.ReadAsync(buffer, cancellationToken);

		/// <inheritdoc/>
		public override Task<Int32> ReadAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken) => baseStream.ReadAsync(buffer, offset, count, cancellationToken);

		/// <inheritdoc/>
		public override Int32 ReadByte() => buffer.Stale ? baseStream.ReadByte() : buffer.Get();

		/// <summary>
		/// Reads a rune from the stream and advances the position within the stream by the encoding byte count, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>The unsigned rune cast to an <see cref="Int32"/>, or -1 if at the end of the stream.</returns>
		public Int32 ReadRune() => helper.ReadRune(this);

		/// <inheritdoc/>
		public override Int64 Seek(Int64 offset, SeekOrigin origin) {
			switch (origin) {
			case SeekOrigin.Current:
				if (buffer.Stale) {
					goto default;
				} else {
					return baseStream.Seek(offset + buffer.Length, origin);
				}
			default:
				return baseStream.Seek(offset, origin);
			}
		}

		/// <inheritdoc/>
		public override void SetLength(Int64 value) => baseStream.SetLength(value);

		/// <inheritdoc/>
		public override String ToString() => baseStream.ToString();

		/// <inheritdoc/>
		public override void Write(ReadOnlySpan<Byte> buffer) => baseStream.Write(buffer);

		/// <inheritdoc/>
		public override void Write(Byte[] buffer, Int32 offset, Int32 count) => baseStream.Write(buffer, offset, count);

		/// <inheritdoc/>
		public override ValueTask WriteAsync(ReadOnlyMemory<Byte> buffer, CancellationToken cancellationToken = default) => baseStream.WriteAsync(buffer, cancellationToken);

		/// <inheritdoc/>
		public override Task WriteAsync(Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken) => baseStream.WriteAsync(buffer, offset, count, cancellationToken);

		/// <inheritdoc/>
		public override void WriteByte(Byte value) => baseStream.WriteByte(value);

		/// <summary>
		/// Fills the buffer.
		/// </summary>
		private void FillBuffer() => buffer.Set(baseStream.ReadByte(), baseStream.ReadByte(), baseStream.ReadByte(), baseStream.ReadByte());
	}
}
