using BDAS2_Restaurace.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.models
{
	internal class HereOrder : Order
	{
		int TableNumber { get; set; }

		public HereOrder(DateTime orderDate, Customer customer, int tableNumber) : base(orderDate, customer)
		{
			TableNumber = tableNumber;	
		}

		public override string ToString()
		{
			string result = "";
			result += "Objednavka na miste\n";
			result += "==================================\n";
			result += $"Cislo stolu: {TableNumber}";
			result += base.ToString();
			return result;
		}
	}
}
