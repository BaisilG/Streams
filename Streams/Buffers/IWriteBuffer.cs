using System;

namespace Stringier.Streams {
	/// <summary>
	/// Represents a read buffer for <see cref="TextStream"/>.
	/// </summary>
	public interface IWriteBuffer {
		/// <summary>
		/// Gets a value that indicates whether the current buffer can be written to.
		/// </summary>
		public Boolean CanWrite { get; }
	}
}
