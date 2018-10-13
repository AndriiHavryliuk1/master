using System.Collections.ObjectModel;
using System.Collections.Generic;
using ConsoleApplication1.Entities;

namespace ConsoleApplication1
{
	public class ItemsDictionary : KeyedCollection<string, Item>
	{
		protected override string GetKeyForItem(Item item)
		{
			return item.Name;
		}

		internal void ConcatItems(IList<Item> frequentItems)
		{
			foreach (var item in frequentItems)
			{
				this.Add(item);
			}
		}
	}
}