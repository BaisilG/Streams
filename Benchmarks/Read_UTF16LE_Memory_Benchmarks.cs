using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Stringier.Streams;

namespace Benchmarks {
	[SimpleJob(RuntimeMoniker.NetCoreApp31)]
	[SimpleJob(RuntimeMoniker.CoreRt31)]
	[MemoryDiagnoser]
	public class Read_UTF16LE_Memory_Benchmarks {
		[Params("The quick brown fox jumps over the lazy dog", "ҐҢЄ QЏЇҪԞ ЬЯФЩЙ ӺФӾ JЏԠPS ФVЄЯ ҬЊЭ LДZҰ DФG")]
		public String Source { get; set; }

		public Stream baseStream;

		public TextStream stream;

		public StreamReader reader;

		public StreamReader stacked;

		[GlobalSetup]
		public void GlobalSetup() {
			baseStream = new StringStream(Source);
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
