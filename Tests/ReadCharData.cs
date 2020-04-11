using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tests {
	public class ReadCharData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new MemoryStream(Array.Empty<Byte>()), null, null, 0, -1, 0, -1, 0, -1, 0, -1, 0, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }), null, null, 0, 'h', 1, 'e', 2, 'l', 3, 'l', 4, 'o' },
			new Object[] { new MemoryStream(new Byte[] { 0xF0, 0x9D, 0x84, 0x9E, 0xEA, 0x93, 0x98, 0x21, 0x21 }), null, null, 0, 0xD834, 4, 0xDD1E, 4, 0xA4D8, 7, '!', 8, '!' },
			new Object[] { new MemoryStream(new Byte[] { 0xEF, 0xBB, 0xBF }), null, null, 3, -1, 3, -1, 3, -1, 3, -1, 3, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xEF, 0xBB, 0xBF, 0x68, 0x65, 0x6C, 0x6C, 0x6F }), null, null, 3, 'h', 4, 'e', 5, 'l', 6, 'l', 7, 'o' },
			new Object[] { new MemoryStream(new Byte[] { 0xEF, 0xBB, 0xBF, 0xF0, 0x9D, 0x84, 0x9E, 0xEA, 0x93, 0x98, 0x21, 0x21 }), null, null, 3, 0xD834, 7, 0xDD1E, 7, 0xA4D8, 10, '!', 11, '!' },
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F }), null, null, 2, 'h', 4, 'e', 6, 'l', 8, 'l', 10, 'o' },
			new Object[] { new MemoryStream(new Byte[] { 0xFE, 0xFF, 0xD8, 0x34, 0xDD, 0x1E, 0xA4, 0xD8, 0x00, 0x21, 0x00, 0x21 }), null, null, 2, 0xD834, 4, 0xDD1E, 6, 0xA4D8, 8, '!', 10, '!' },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE }), null, null, 2, -1, 2, -1, 2, -1, 2, -1, 2, -1 },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00, 0x6C, 0x00, 0x6C, 0x00, 0x6F, 0x00 }), null, null, 2, 'h', 4, 'e', 6, 'l', 8, 'l', 10, 'o' },
			new Object[] { new MemoryStream(new Byte[] { 0xFF, 0xFE, 0x34, 0xD8, 0x1E, 0xDD, 0xD8, 0xA4, 0x21, 0x00, 0x21, 0x00 }), null, null, 2, 0xD834, 4, 0xDD1E, 6, 0xA4D8, 8, '!', 10, '!' },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
