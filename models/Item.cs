using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2SemestralkaDemo.models
{
	internal abstract class Item
	{
		public string Name { get; }
		public double Price { get; }

		public Item(string name, double price)
		{
			Name = name;
			Price = price;
		}

		public override string ToString()
		{
			string result = "\n";
			result += $"Nazev: {Name}\n";
			result += $"Cena: {Price}\n";
			return result;
		}
	}
}
