using System;
using ConsoleApplication1.Contracts;
using ConsoleApplication1.Implementation;

namespace ConsoleApplication1.Entities
{
	public class Rule : IComparable<Rule>
	{
		readonly string combination, remaining;
		readonly double confidence;

		public Rule(string combination, string remaining, double confidence)
		{
			this.combination = combination;
			this.remaining = remaining;
			this.confidence = confidence;
		}

		public string X { get { return combination; } }

		public string Y { get { return remaining; } }

		public double Confidence { get { return confidence; } }

		public int CompareTo(Rule other)
		{
			return X.CompareTo(other.X);
		}

		public override int GetHashCode()
		{
			ISorter sorter = new Sorter();
			string sortedXY = sorter.Sort(X + Y);
			return sortedXY.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var other = obj as Rule;
			if (other == null)
			{
				return false;
			}

			return other.X == this.X && other.Y == this.Y ||
			       other.X == this.Y && other.Y == this.X;
		}
	}
}