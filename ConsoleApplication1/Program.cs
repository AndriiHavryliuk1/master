using System;
using System.Diagnostics;
using System.IO;
using ConsoleApplication1.Contracts;
using ConsoleApplication1.Implementation;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] items = ReadFromFile("items.txt");
			string[] itemsD = null; // ReadFromFile("itemsD.txt");
			string[] transactions = ReadFromFile("transactions.txt");
			IApriori apriori = new Apriori();
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			var result = apriori.ProcessTransaction(0.4, 0.5, items, transactions, itemsD);
			stopWatch.Stop();
			Console.Write("Closed itemsets:");
			foreach (var closedItem in result.ClosedItemSets)
			{
				Console.Write($"\nKey: {closedItem.Key}\t");
				Console.WriteLine($"Values: \t");
				foreach (var value in closedItem.Value)
				{
					Console.Write($"{{ key: {value.Key}, value: {value.Value}}}, ");
				}
			}
			Console.WriteLine("Frequent items:");
			foreach (var irequentItem in result.FrequentItems)
			{
				Console.WriteLine($"Name: {irequentItem.Name}, Support: {irequentItem.Support}");
			}
			Console.WriteLine("Maximall itemsets:");
			foreach (var maximalItemSet in result.MaximalItemSets)
			{
				Console.WriteLine($"{maximalItemSet}");
			}

			Console.WriteLine($"Found {result.StrongRules.Count} strong rules:");
			foreach (var strongRule in result.StrongRules)
			{
				Console.WriteLine($"{{{strongRule.X} -> {strongRule.Y}}}, confidence: {strongRule.Confidence}");
			}
			Console.WriteLine($"Processing time: {stopWatch.Elapsed}");
			Console.WriteLine($"All transactions: {transactions.Length}");
			Console.ReadKey();
		}

		private static string[] ReadFromFile(string path)
		{
			var text = File.ReadAllText(path);
			// char[] delimiterChars = { ' ', ',', '.', '\t', '\n', '\\', '\"' };
			text = text.Replace(" ", "");
			text = text.Replace("\"", "");
			text = text.Replace("\n", "");
			var words = text.Split(',');
			return words;
		}
	}
}