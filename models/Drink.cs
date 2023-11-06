using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2SemestralkaDemo.models
{
	internal class Drink : Item
	{
		public double Volume { get; set; }

		public Drink(string name, double price, double volume) : base(name, price)
		{
			Volume = volume;
		}

		public override string ToString()
		{
			string result = "Napoj\n-------------------";
			result += base.ToString();
			result += $"Objem: {Volume}\n";
			return result;
		}
	}
}
