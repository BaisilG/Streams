using System;
using System.Collections;
using System.Collections.Generic;
using Stringier.Streams;

namespace Tests {
	public class ReadCharData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new StringStream("\uFEFF"), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new StringStream("\uFEFFhello world"), null, null, 2, 'h', 4, 'e', 6, 'l', 8, 'l', 10, 'o' },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
