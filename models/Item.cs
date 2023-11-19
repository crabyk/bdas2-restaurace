using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.models
{
	internal abstract class Item
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }

		public Item(int id, string name, double price)
		{
			ID = id;
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
