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
		[Params("\uFEFFThe quick brown fox jumps over the lazy dog", "\uFEFFҐҢЄ QЏЇҪԞ ЬЯФЩЙ ӺФӾ JЏԠPS ФVЄЯ ҬЊЭ LДZҰ DФG", "\uFEFFֵ ٻ Њ ђ ϵ հ ӡ ט ѵ ؇ Ӌ Ҫ Ϲ ҕ ّ ԥ ׇ ϓ א Ͼ ה ظ ֕ צ ϩ Ѓ ׳ ؅ Է ת г Ҝ ϱ ؄ ש ٵ ԝ Ր ԍ Ѻ ، ق ؝ ҫ Ӭ ύ Ҷ ӹ С ؇ Р ш ֪ ؅ ٟ Ϫ ָ т ؆ ٶ ϫ ӧ ٽ Џ Б Л ؘ ױ υ Ӗ Ӕ ؑ ӈ ֬ Ѻ ϴ ך ע Ж Ҿ ؆ ԧ Ҡ Ժ ؄ ټ Ԉ Ͼ ԑ ҩ أ ԋ ԟ Ϗ ϑ ђ Ճ ؙ ќ Қ ծ ѻ ٧ ْ ә ј ײ լ ԁ ՞ ѧ Ѝ ש ԉ Ӊ Ϻ Ԥ Ф ٨ Ҳ ҇ ؘ ү َ ϭ φ ٥ Ѐ ְ ӆ б Е ٺ و ַ ՛ ֘ غ ٓ Т ٹ ҃ ֕ ع ף Ӑ ז ׎ ٬ Ҕ ז խ Ϯ ׎ ԛ к ӎ ֛ ֱ Ի қ բ ϥ ٱ В ׃ ٽ ؘ Љ ԧ Ќ ԏ ҩ Ӿ Ԝ ٱ ϣ ؊ թ Է ӵ ѱ Ӈ ӷ ؞ ء ϟ к Ϲ ֈ ։ פ ω ։ л ե ח ԓ پ Զ")]
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
