using System;
using ConsoleApplication1.Contracts;

namespace ConsoleApplication1.Implementation
{
	public class Sorter : ISorter
	{
		string ISorter.Sort(string token)
		{
			var tokenArray = token.ToCharArray();
			Array.Sort(tokenArray);
			return new string(tokenArray);
		}
	}
}
