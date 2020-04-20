using System;

namespace Stringier.Streams.Buffers {
	internal interface IWriteBuffer : IBuffer {
		/// <summary>
		/// Gets a value indicating whether the buffer can be written to.
		/// </summary>
		public Boolean CanWrite { get; }
	}
}
