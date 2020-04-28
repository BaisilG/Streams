using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tests {
	public class WriteByteData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new MemoryStream(), null, null, new Int32[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 } },
			new Object[] { new MemoryStream(), null, null, new Int32[] { 0x68, 0x68, 0x68, 0x00, 0x65, 0x00, 0x00, 0x6C, 0x6C } },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
