using System;
using BenchmarkDotNet.Running;
using Consolator.UI;
using Consolator.UI.Theming;
using Console = Consolator.Console;

namespace Benchmarks {
	public class Program {
		internal readonly static KeyChoiceSet MenuChoices = new KeyChoiceSet(" Enter Choice: ",
			new KeyChoice(ConsoleKey.D1, "Read UTF-16LE File", () => BenchmarkRunner.Run<Read_UTF16LE_File_Benchmarks>()),
			new KeyChoice(ConsoleKey.D2, "Read UTF-16LE Memory", () => BenchmarkRunner.Run<Read_UTF16LE_Memory_Benchmarks>()),
			new KeyChoice(ConsoleKey.D3, "StringStream", () => BenchmarkRunner.Run<StringStreamBenchmarks>()),
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
