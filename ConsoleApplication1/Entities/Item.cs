using System;
namespace ConsoleApplication1.Entities
{
	public class Item : IComparable<Item>
	{
		public string Name { get; set; }
		public double Support { get; set; }


		public int CompareTo(Item other)
		{
			return Name.CompareTo(other.Name);
		}
	}
}