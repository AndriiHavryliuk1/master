using ConsoleApplication1.Entities;

namespace ConsoleApplication1.Contracts
{
	using System.Collections.Generic;

	public interface IApriori
	{
		Output ProcessTransaction(double minSupport, double minConfidence, IEnumerable<string> items, string[] transactions, string[] itemsD = null);
	}
}