using System;
using System.Collections;
using System.Collections.Generic;
using Stringier.Streams;

namespace Tests {
	public class ReadRuneData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new StringStream("\uFEFF"), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new StringStream("\uFEFFG\uD834\uDD1Eabc"), null, null, 2, 'G', 4, 0x01D11E, 8, 'a', 10, 'b', 12, 'c' },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
