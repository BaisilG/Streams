using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tests {
	public class ReadRuneData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			// UTF-8
			new Object[] { new MemoryStream(Array.Empty<Byte>()), null, null, 0, -1, 0, -1, 0, -1, 0, -1, 0, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0x47, 0xF0, 0x9D, 0x84, 0x9E, 0x61, 0x62, 0x63 }), null, null, 0, 'G', 1, 0x01D11E, 5, 'a', 6, 'b', 7, 'c' },
			// UTF-8 BOM
			new Object[] { new MemoryStream(new Byte[] { 0xEF, 0xBB, 0xBF }), null, null, 3, -1, 3, -1, 3, -1, 3, -1, 3, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xEF, 0xBB, 0xBF, 0x47, 0xF0, 0x9D, 0x84, 0x9E, 0x61, 0x62, 0x63 }), null, null, 3, 'G', 4, 0x01D11E, 8, 'a', 9, 'b', 10, 'c' },
			// UTF-16 BE
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF, 0x00, 0x47, 0xD8, 0x34, 0xDD, 0x1E, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63 }), null, null, 2, 'G', 4, 0x01D11E, 8, 'a', 10, 'b', 12, 'c' },
			// UTF-16 LE
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x47, 0x00, 0x34, 0xD8, 0x1E, 0xDD, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00 }), null, null, 2, 'G', 4, 0x01D11E, 8, 'a', 10, 'b', 12, 'c' },
			// UTF-32 BE
			new Object[] { new MemoryStream(new Byte[] { 0x00, 0x00, 0xFE, 0xFF }), null, null, 4, -1, 4, -1, 4, -1, 4, -1, 4, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0x00, 0x00, 0xFE, 0xFF, 0x00, 0x00, 0x00, 0x47, 0x00, 0x01, 0xD1, 0x1E, 0x00, 0x00, 0x00, 0x61, 0x00, 0x00, 0x00, 0x62, 0x00, 0x00, 0x00, 0x63 }), null, null, 4, 'G', 8, 0x01D11E, 12, 'a', 16, 'b', 20, 'c' },
			// UTF-32 LE
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x00, 0x00 }), null, null, 4, -1, 4, -1, 4, -1, 4, -1, 4, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x00, 0x00, 0x47, 0x00, 0x00, 0x00, 0x1E, 0xD1, 0x01, 0x00, 0x61, 0x00, 0x00, 0x00, 0x62, 0x00, 0x00, 0x00, 0x63, 0x00, 0x00, 0x00 }), null, null, 4, 'G', 8, 0x01D11E, 12, 'a', 16, 'b', 20, 'c' },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
