using System;
using System.IO;

namespace Stringier.Streams {
	/// <summary>
	/// Represents a read buffer for <see cref="TextStream"/>.
	/// </summary>
	public interface IWriteBuffer {
		/// <summary>
		/// Gets a value that indicates whether the current buffer can be written to.
		/// </summary>
		public Boolean CanWrite { get; }

		/// <summary>
		/// The length of the buffer; the amount of bytes it is currently holding.
		/// </summary>
		public Int32 Length { get; set; }

		/// <summary>
		/// The stream being buffered.
		/// </summary>
		public Stream Stream { get; set; }
	}
}
