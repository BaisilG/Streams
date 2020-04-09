using System;
using BenchmarkDotNet.Running;
using Consolator.UI;
using Consolator.UI.Theming;
using Console = Consolator.Console;

namespace Benchmarks {
	public class Program {
		internal readonly static KeyChoiceSet MenuChoices = new KeyChoiceSet(" Enter Choice: ",
			new KeyChoice(ConsoleKey.D1, "ReadChar UTF-8 File", () => BenchmarkRunner.Run<ReadChar_UTF8_File_Benchmarks>()),
			new KeyChoice(ConsoleKey.D2, "ReadChar UTF-8 Memory", () => BenchmarkRunner.Run<ReadChar_UTF8_Memory_Benchmarks>()),
			new KeyChoice(ConsoleKey.D3, "ReadChar UTF-16LE File", () => BenchmarkRunner.Run<ReadChar_UTF16LE_File_Benchmarks>()),
			new KeyChoice(ConsoleKey.D4, "ReadChar UTF-16LE Memory", () => BenchmarkRunner.Run<ReadChar_UTF16LE_Memory_Benchmarks>()),
			new KeyChoice(ConsoleKey.D5, "ReadRune UTF-8 File", () => BenchmarkRunner.Run<ReadRune_UTF8_File_Benchmarks>()),
			new KeyChoice(ConsoleKey.D6, "ReadRune UTF-8 Memory", () => BenchmarkRunner.Run<ReadRune_UTF8_Memory_Benchmarks>()),
			new KeyChoice(ConsoleKey.D7, "StringStream", () => BenchmarkRunner.Run<StringStreamBenchmarks>()),
			new BackKeyChoice(ConsoleKey.Q, "Quit", () => Environment.Exit(0)));

		public static void Main() {
			Theme.DefaultDark.Apply();

			while (true) {
				Console.WriteChoices(MenuChoices);
				Console.ReadChoice(MenuChoices);
			}
		}
	}
}
