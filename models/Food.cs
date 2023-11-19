using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.models
{
	internal class Food : Item
	{
		public double Weight { get; set; }
		public string Recipe { get; set; }

		public Food (int id, string name, double price, double weight, string recipe) : base(id, name, price)
		{
			Weight = weight;
			Recipe = recipe;
		}

		public override string ToString()
		{
			string result = "Jidlo\n-------------------";
			result += base.ToString();
			result += $"Gramaz: {Weight}\n";
			result += $"Recept/Popis: {Recipe}\n";
			return result;
		}
	}
}
