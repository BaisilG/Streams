using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Stringier.Streams;

namespace Tests {
	public class WriteRuneData : IEnumerable<Object[]> {
		private readonly IEnumerable<Object[]> data = new List<Object[]> {
			new Object[] { new MemoryStream(), null, null, Encoding.UTF8, new Int32[] { -1 } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF8, new Int32[] { 'h', 'e', 'l', 'l', 'o' } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF16BE, new Int32[] { -1 } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF16BE, new Int32[] { 'п', 'р', 'и', 'в', 'е', 'т' } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF16LE, new Int32[] { -1 } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF16LE, new Int32[] { 'п', 'р', 'и', 'в', 'е', 'т' } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF32BE, new Int32[] { -1 } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF32BE, new Int32[] { 'п', 'р', 'и', 'в', 'е', 'т' } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF32LE, new Int32[] { -1 } },
			new Object[] { new MemoryStream(), null, null, Encoding.UTF32LE, new Int32[] { 'п', 'р', 'и', 'в', 'е', 'т' } },
		};

		public IEnumerator<Object[]> GetEnumerator() => data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();
	}
}
