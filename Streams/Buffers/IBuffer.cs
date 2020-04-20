using System;
using System.IO;

namespace Stringier.Streams.Buffers {
	internal interface IBuffer : IEquatable<Byte[]> {
		/// <summary>
		/// Gets a value indicating whether the buffer can be seeked.
		/// </summary>
		public Boolean CanSeek { get; }

		/// <summary>
		/// The length of the buffer; the amount of bytes it is currently holding.
		/// </summary>
		public Int32 Length { get; set; }

		/// <summary>
		/// Whether data in the buffer is stale.
		/// </summary>
		public Boolean Stale {
			get => Length == 0;
			set {
				if (value) {
					Length = 0;
				}
			}
		}

		/// <summary>
		/// The stream being buffered.
		/// </summary>
		/// <exception cref="NullReferenceException">The buffer is null; the buffer hasn't been registered with a stream.</exception>
		/// <exception cref="InvalidOperationException">The buffer has already been registered and doesn't support multiple stream registrations.</exception>
		internal Stream Stream { get; set; }

		/// <summary>
		/// Shifts the contents of the buffer by <paramref name="amount"/>.
		/// </summary>
		/// <param name="amount">The amount of bytes to shift.</param>
		/// <remarks>
		/// This is used to move the current position within the buffer. It does not necessarily actually shift the contents, but will appear to.
		/// </remarks>
		public void Shift(Int32 amount);
	}
}