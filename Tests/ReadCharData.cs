using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tests {
	public class ReadCharData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F }), null, null, 2, 'h', 4, 'e', 6, 'l', 8, 'l', 10, 'o' },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00 }), null, null, 2, 'h', 4, 'e', 6, 'l', 8, 'l', 10, 'o' },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
