using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tests {
	public class ReadSpanData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new MemoryStream(Array.Empty<Byte>()), null, null, 0, new Byte[] { 0, 0, 0, 0 }, 0 , new Byte[] { 0, 0, 0, 0}, 0, new Byte[] { 0, 0, 0, 0 }, 0 },
			new Object[] { new MemoryStream(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F }), null, null, 0, new Byte[] { 0x68, 0x65, 0x6C, 0x6C }, 4, new Byte[] { 0x6F, 0, 0, 0 }, 5, new Byte[] { 0, 0, 0, 0 }, 5 },
			new Object[] { new MemoryStream(new Byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x77, 0x6F, 0x72, 0x6C, 0x64 }), null, null, 0, new Byte[] { 0x68, 0x65, 0x6C, 0x6C }, 4, new Byte[] { 0x6F, 0x20, 0x77, 0x6F }, 8, new Byte[] { 0x72, 0x6C, 0x64, 0 }, 11 },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
