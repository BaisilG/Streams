using System;
using System.Collections;
using System.Collections.Generic;
using Stringier.Streams;

namespace Tests {
	public class PeekByteData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new StringStream(""), null, null, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			new Object[] { new StringStream("hello world"), null, null, 0x68, 0x68, 0x68, 0x00, 0x65, 0x00, 0x00, 0x6C, 0x6C },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
