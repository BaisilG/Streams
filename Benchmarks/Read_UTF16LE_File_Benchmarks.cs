using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Stringier.Streams;

namespace Benchmarks {
	[SimpleJob(RuntimeMoniker.NetCoreApp31)]
	[SimpleJob(RuntimeMoniker.CoreRt31)]
	[MemoryDiagnoser]
	public class Read_UTF16LE_File_Benchmarks {
		[Params("UTF16LE_English.txt", "UTF16LE_FakeRussian.txt")]
		public String File { get; set; }

		public Stream baseStream;

		public TextStream stream;

		public StreamReader reader;

		public StreamReader stacked;

		[GlobalSetup]
		public void GlobalSetup() {
			baseStream = new FileStream(File, FileMode.Open);
			stream = new TextStream(baseStream);
			reader = new StreamReader(baseStream);
			stacked = new StreamReader(stream);
		}

		[Benchmark]
		public void TextStream() {
			while (stream.ReadChar() >= 0) { }
		}

		[Benchmark]
		public void StreamReader() => reader.ReadToEnd();

		[Benchmark]
		public void Stacked() => stacked.ReadToEnd();
	}
}
