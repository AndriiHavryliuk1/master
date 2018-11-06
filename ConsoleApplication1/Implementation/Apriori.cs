using System;
using ConsoleApplication1.Contracts;
using ConsoleApplication1.Entities;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using JulMar.Core.Extensions;

namespace ConsoleApplication1.Implementation
{
	public class Apriori : IApriori
	{
		readonly ISorter _sorter;

		public Apriori()
		{
			_sorter = new Sorter();
		}

		Output IApriori.ProcessTransaction(double minSupport, double minConfidence, IEnumerable<string> items, string[][] transactions, string[] itemsD = null)
		{
			IList<Item> frequentItems = GetL1FrequentItems(minSupport, items, transactions);
			ItemsDictionary allFrequentItems = new ItemsDictionary();
			allFrequentItems.ConcatItems(frequentItems);
			var candidates = new Dictionary<string[], double>();
			double transactionsCount = transactions.Count();

			do
			{
				candidates = GenerateCandidates(frequentItems, transactions, itemsD);
				frequentItems = GetFrequentItems(candidates, minSupport, transactionsCount);
				allFrequentItems.ConcatItems(frequentItems);
			} while (candidates.Count != 0);

			HashSet<Rule> rules = GenerateRules(allFrequentItems);
			IList<Rule> strongRules = GetStrongRules(minConfidence, rules, allFrequentItems);
			//Dictionary<string, Dictionary<string, double>> closedItemSets = GetClosedItemSets(allFrequentItems);
		//	IList<string> maximalItemSets = GetMaximalItemSets(closedItemSets);

			return new Output
			{
				StrongRules = strongRules,
				MaximalItemSets = null,
				ClosedItemSets = null,
				FrequentItems = allFrequentItems
			};
		}

		private List<Item> GetL1FrequentItems(double minSupport, IEnumerable<string> items, string[][] transactions)
		{
			var frequentItemsL1 = new List<Item>();
			double transactionsCount = transactions.Count();

			foreach (var item in items)
			{
				double support = GetSupport(new [] {item}, transactions);

				if (support / transactionsCount >= minSupport)
				{
					frequentItemsL1.Add(new Item { Names = new [] { item }, Support = support });
				}
			}
		//	frequentItemsL1.Sort();
			return frequentItemsL1;
		}

		private double GetSupport(string[] generatedCandidates, IEnumerable<IEnumerable<string>> transactionsList)
		{
			double support = 0;

			foreach (var transactions in transactionsList)
			{
				if (!generatedCandidates.Except(transactions).Any())
				{
					support++;
				}
			}

			return support;
		}

		private bool CheckIsSubset(string child, string parent)
		{
			foreach (char c in child)
			{
				if (!parent.Contains(c))
				{
					return false;
				}
			}

			return true;
		}

		private Dictionary<string[], double> GenerateCandidates(IList<Item> frequentItems, IEnumerable<IEnumerable<string>> transactions, string[] itemsD)
		{
			var candidates = new Dictionary<string[], double>();

			for (var i = 0; i < frequentItems.Count - 1; i++)
			{
				var firstItems = frequentItems[i].Names.OrderBy(x => x).ToArray();
				
				for (var j = i + 1; j < frequentItems.Count; j++)
				{
					var secondItems = frequentItems[j].Names.OrderBy(x => x).ToArray();
					var generatedCandidate = GenerateCandidate(firstItems, secondItems);


					double support = GetSupport(generatedCandidate, transactions);
					if (itemsD == null)
					{
						candidates.Add(generatedCandidate, support);
					}
					else
					{
						if (itemsD.Any(itemD => generatedCandidate.Contains(itemD)))
						{
							candidates.Add(generatedCandidate, support);
						}
					}
				}

			}

			return candidates;
		}

		private string[] GenerateCandidate(string[] firstItems, string[] secondItems)
		{
			return firstItems.Concat(secondItems).ToArray();
		}

		private List<Item> GetFrequentItems(IDictionary<string[], double> candidates, double minSupport, double transactionsCount)
		{
			var frequentItems = new List<Item>();

			foreach (var item in candidates)
			{
				if (item.Value / transactionsCount >= minSupport)
				{
					frequentItems.Add(new Item { Names = item.Key, Support = item.Value });
				}
			}

			return frequentItems;
		}

		/*

		private Dictionary<string, Dictionary<string, double>> GetClosedItemSets(ItemsDictionary allFrequentItems)
		{
			var closedItemSets = new Dictionary<string, Dictionary<string, double>>();
			int i = 0;

			foreach (var item in allFrequentItems)
			{
				Dictionary<string, double> parents = GetItemParents(item.Names, ++i, allFrequentItems);

				if (CheckIsClosed(item.Name, parents, allFrequentItems))
				{
					closedItemSets.Add(item.Name, parents);
				}
			}

			return closedItemSets;
		}

		private Dictionary<string, double> GetItemParents(string child, int index, ItemsDictionary allFrequentItems)
		{
			var parents = new Dictionary<string, double>();

			for (int j = index; j < allFrequentItems.Count; j++)
			{
				string parent = allFrequentItems[j].Name;

				if (parent.Length == child.Length + 1)
				{
					if (CheckIsSubset(child, parent))
					{
						parents.Add(parent, allFrequentItems[parent].Support);
					}
				}
			}

			return parents;
		}
/*
		private bool CheckIsClosed(string[] child, Dictionary<string, double> parents, ItemsDictionary allFrequentItems)
		{
			foreach (string parent in parents.Keys)
			{
				if (allFrequentItems[child].Support == allFrequentItems[parent].Support)
				{
					return false;
				}
			}

			return true;
		}

	*/

		private IList<string> GetMaximalItemSets(Dictionary<string, Dictionary<string, double>> closedItemSets)
		{
			var maximalItemSets = new List<string>();

			foreach (var item in closedItemSets)
			{
				Dictionary<string, double> parents = item.Value;

				if (parents.Count == 0)
				{
					maximalItemSets.Add(item.Key);
				}
			}

			return maximalItemSets;
		}

		private HashSet<Rule> GenerateRules(ItemsDictionary allFrequentItems)
		{
			var rulesList = new HashSet<Rule>();

			foreach (var item in allFrequentItems)
			{
				if (item.Names.Length <= 1) continue;
				IEnumerable<string> subsetsList = item.Names;

				foreach (var subset in subsetsList)
				{
					string[] remaining = GetRemaining(subset, item.Names);
					Rule rule = new Rule(new [] { subset }, remaining, 0);

					if (!rulesList.Contains(rule))
					{
						rulesList.Add(rule);
					}
				}
			}

			return rulesList;
		}

		private IEnumerable<string> GenerateSubsets(string item)
		{
			IEnumerable<string> allSubsets = new string[] { };
			int subsetLength = item.Length / 2;

			for (int i = 1; i <= subsetLength; i++)
			{
				IList<string> subsets = new List<string>();
				GenerateSubsetsRecursive(item, i, new char[item.Length], subsets);
				allSubsets = allSubsets.Concat(subsets);
			}

			return allSubsets;
		}

		private void GenerateSubsetsRecursive(string item, int subsetLength, char[] temp, IList<string> subsets, int q = 0, int r = 0)
		{
			if (q == subsetLength)
			{
				StringBuilder sb = new StringBuilder();

				for (int i = 0; i < subsetLength; i++)
				{
					sb.Append(temp[i]);
				}

				subsets.Add(sb.ToString());
			}

			else
			{
				for (int i = r; i < item.Length; i++)
				{
					temp[q] = item[i];
					GenerateSubsetsRecursive(item, subsetLength, temp, subsets, q + 1, i + 1);
				}
			}
		}

		private string[] GetRemaining(string child, string[] parent)
		{
			return parent.Where(x => x != child).ToArray();
		}

		private IList<Rule> GetStrongRules(double minConfidence, HashSet<Rule> rules, ItemsDictionary allFrequentItems)
		{
			var strongRules = new List<Rule>();

			foreach (Rule rule in rules)
			{
				var xy = rule.X.Concat(rule.Y).ToArray();
				AddStrongRule(rule, xy, strongRules, minConfidence, allFrequentItems);
			}

			//strongRules.Sort();
			return strongRules;
		}

		private void AddStrongRule(Rule rule, string[] XY, List<Rule> strongRules, double minConfidence, ItemsDictionary allFrequentItems)
		{
			double confidence = GetConfidence(rule.X, XY, allFrequentItems);

			if (confidence >= minConfidence)
			{
				Rule newRule = new Rule(rule.X, rule.Y, confidence);
				strongRules.Add(newRule);
			}
		}

		private double GetConfidence(string[] X, string[] XY, ItemsDictionary allFrequentItems)
		{
			var XYstring= string.Join("", XY.OrderBy(x => x));
			var Xstring = string.Join("", X.OrderBy(x => x));
			var supportX = allFrequentItems.FirstOrDefault(x => string.Join("", x.Names.OrderBy(k => k)) == Xstring).Support;
			var supportXY = allFrequentItems.FirstOrDefault(x => string.Join("", x.Names.OrderBy(k => k)) == XYstring).Support;
			return supportXY / supportX;
		}
	}
}