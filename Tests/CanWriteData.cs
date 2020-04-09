using System;
using System.Collections;
using System.Collections.Generic;
using Stringier.Streams;

namespace Tests {
	public class CanWriteData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new StringStream("\uFEFF"), null, null, true },
			new Object[] { new StringStream("\uFEFFhello world"), null, null, true },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
