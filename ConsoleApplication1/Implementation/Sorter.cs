using System;
using ConsoleApplication1.Contracts;

namespace ConsoleApplication1.Implementation
{
	public class Sorter : ISorter
	{
		string[] ISorter.Sort(string[] tokens)
		{
			Array.Sort(tokens);
			return tokens;
		}
	}
}
