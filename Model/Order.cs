using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	public class Order
	{
		public int ID { get; set; }
		public DateTime OrderDate { get; set; }
		public Payment Payment { get; set; }

		public List<Item> Items { get; set; }
		public Customer Customer { get; set; }

		public Address Address { get; set; }	

		public Table Table { get; set; }

		/*
		public Order(DateTime orderDate, Customer customer)
		{
			OrderDate = orderDate;
			Customer = customer;
			Items = new List<Item>();
		}
		*/

		public Item this[int index]
		{
			get
			{
				if (index >= 0 && index < Items.Count)
					return Items[index];
				else
					throw new IndexOutOfRangeException("Index at item is out of range");
			}
		}

		public void AddItem(Item item)
		{
			Items.Add(item);
		}

		public void AddItem(Item item, int amount)
		{
			for (int i = 0; i < amount; i++)
				AddItem(item);
		}

		public override string ToString()
		{
			string result = "";
			result += "\nDatum: " + OrderDate.ToString();
			result += "\n-----------------------------------------------------------";
			result += "\n Zakaznik";
			result += "\n" + Customer.ToString();
			result += $"\nPolozky objednavky ({Items.Count})";
			result += "\n-----------------------------------------------------------";
			int cnt = 1;
			foreach (Item item in Items)
			{
				result += $"\n{cnt})" + item.ToString();
				cnt++;
			}
			result += "\n===========================================================";

			return result;
		}
	}
}
