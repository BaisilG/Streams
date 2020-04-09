using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Stringier.Streams;

namespace Benchmarks {
	[SimpleJob(RuntimeMoniker.NetCoreApp31)]
	[SimpleJob(RuntimeMoniker.CoreRt31)]
	[MemoryDiagnoser]
	public class ReadChar_UTF8_File_Benchmarks {
		[Params("UTF8_English.txt", "UTF8_FakeRussian.txt", "UTF8_Random.txt")]
		public String File { get; set; }

		public Stream baseStream;

		public TextStream stream;

		public StreamReader reader;

		[GlobalSetup]
		public void GlobalSetup() {
			baseStream = new FileStream(File, FileMode.Open);
			stream = new TextStream(baseStream);
			reader = new StreamReader(baseStream);
		}

		[Benchmark]
		public void TextStream() {
			while (stream.ReadChar() >= 0) { }
		}

		[Benchmark]
		public void StreamReader() => reader.ReadToEnd();
	}
}
