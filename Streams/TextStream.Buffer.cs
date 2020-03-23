using System;
using Defender;

namespace Stringier.Streams {
	public sealed partial class TextStream {
		/// <summary>
		/// Provides a safe and reusable buffer for <see cref="TextStream"/>.
		/// </summary>
		/// <remarks>
		/// This entire thing is required for peeking and reading the BOM. It's not a performance buffer, so it's very small.
		/// </remarks>
		internal class Buffer : IEquatable<Byte[]> {
			private readonly Int32[] buffer = new Int32[4];

			public Boolean Stale {
				get => Length == 0;
				set {
					if (value) {
						Length = 0;
					}
				}
			}

			public Int32 Length { get; internal set; } = 0;

			public static Boolean operator ==(Buffer left, Byte[] right) => left.Equals(right);

			public static Boolean operator !=(Buffer left, Byte[] right) => !left.Equals(right);

			public static Buffer operator <<(Buffer buffer, Int32 amount) {
				buffer.Shift(amount);
				return buffer;
			}

			public Boolean Equals(Byte[] other) {
				if (Length < other.Length) {
					return false;
				}
				for (Int32 i = 0; i < other.Length; i++) {
					if (buffer[i] != other[i]) {
						return false;
					}
				}
				return true;
			}

			public Int32 Get() {
				Int32 result = Peek();
				if (Length > 1) {
					Shift(1);
				} else {
					Length = 0;
				}
				return result;
			}

			public void Get(out Int32 value) {
				Peek(out value);
				Length = 0;
			}

			public void Get(out Int32 first, out Int32 second) {
				Peek(out first, out second);
				Length = 0;
			}

			public void Get(out Int32 first, out Int32 second, out Int32 third) {
				Peek(out first, out second, out third);
				Length = 0;
			}

			public void Get(out Int32 first, out Int32 second, out Int32 third, out Int32 fourth) {
				Peek(out first, out second, out third, out fourth);
				Length = 0;
			}

			public Int32 Peek() => buffer[0];

			public void Peek(out Int32 value) {
				value = buffer[0];
			}

			public void Peek(out Int32 first, out Int32 second) {
				first = buffer[0];
				second = buffer[0];
			}

			public void Peek(out Int32 first, out Int32 second, out Int32 third) {
				first = buffer[0];
				second = buffer[1];
				third = buffer[2];
			}

			public void Peek(out Int32 first, out Int32 second, out Int32 third, out Int32 fourth) {
				first = buffer[0];
				second = buffer[1];
				third = buffer[2];
				fourth = buffer[3];
			}

			public void Set(Int32 value) {
				buffer[0] = value;
				Length = 1;
			}

			public void Set(Int32 first, Int32 second) {
				buffer[0] = first;
				buffer[1] = second;
				Length = 2;
			}

			public void Set(Int32 first, Int32 second, Int32 third) {
				buffer[0] = first;
				buffer[1] = second;
				buffer[2] = third;
				Length = 3;
			}

			public void Set(Int32 first, Int32 second, Int32 third, Int32 fourth) {
				buffer[0] = first;
				buffer[1] = second;
				buffer[2] = third;
				buffer[3] = fourth;
				Length = 4;
			}

			public void Shift(Int32 amount) {
				if (amount > 4) {
					amount = 4;
				}
				for (Int32 i = 0; i < amount; i++) {
					buffer[0] = buffer[1];
					buffer[1] = buffer[2];
					buffer[2] = buffer[3];
					Length--;
				}
			}

			public override String ToString() => $"[{buffer[0]}, {buffer[1]}, {buffer[2]}, {buffer[3]}]";
		}
	}
}
