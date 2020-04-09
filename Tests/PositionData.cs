using System;
using System.Collections;
using System.Collections.Generic;
using Stringier.Streams;

namespace Tests {
	public class PositionData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new StringStream("\uFEFF"), null, null, 2, 2, 2, 2, 2, 2, 0, 1, 2, 2, 2, 2, 2, 2, 2 },
			new Object[] { new StringStream("\uFEFFhello world"), null, null, 2, 3, 4, 5, 6, 7, 0, 1, 2, 2, 2, 3, 3, 4, 5 },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
