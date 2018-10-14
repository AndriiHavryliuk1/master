﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
			var transactions = new string[5][];
			transactions[0] = new[] { "a", "b", "c" };
			transactions[1] = new[] { "a", "b", "e" };
			transactions[2] = new[] { "a", "b" };
			transactions[3] = new[] { "a", "c" };
			transactions[4] = new[] { "b", "d" };

			IApriori apriori = new Apriori();
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			var result = apriori.ProcessTransaction(0.4, 0.5, items, transactions, itemsD);
			stopWatch.Stop();
			if (result.ClosedItemSets != null)
			{
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
			}
			Console.WriteLine("Frequent items:");
			foreach (var frequentItem in result.FrequentItems)
			{
				Console.WriteLine($"Name: { string.Join(" ",frequentItem.Names)}, Support: {frequentItem.Support}");
			}
			if (result.MaximalItemSets != null)
			{
				Console.WriteLine("Maximall itemsets:");
				foreach (var maximalItemSet in result.MaximalItemSets)
				{
					Console.WriteLine($"{maximalItemSet}");
				}
			}

			Console.WriteLine($"Found {result.StrongRules.Count} strong rules:");
			foreach (var strongRule in result.StrongRules)
			{
				Console.WriteLine($"{{{string.Join(" ", strongRule.X)} -> {string.Join(" ", strongRule.Y)}}}, confidence: {strongRule.Confidence}");
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