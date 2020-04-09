using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tests {
	public class ReadRuneData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF, 0x00, 0x47, 0xD8, 0x34, 0xDD, 0x1E, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63 }), null, null, 2, 'G', 4, 0x01D11E, 8, 'a', 10, 'b', 12, 'c' },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x47, 0x00, 0x34, 0xD8, 0x1E, 0xDD, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00 }), null, null, 2, 'G', 4, 0x01D11E, 8, 'a', 10, 'b', 12, 'c' },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
