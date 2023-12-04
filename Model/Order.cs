using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Order : ModelBase
    {
        private DateTime orderDate;
        private Payment payment;
        private List<Item> items;
        private Customer customer;
        private Table? table;
        private Address? address;

        public DateTime OrderDate
        {
            get { return orderDate; }
            set
            {
                orderDate = value;
                RaisePropertyChanged(nameof(OrderDate));
            }
        }
        public Payment Payment
        {
            get { return payment; }
            set
            {
                payment = value;
                RaisePropertyChanged(nameof(Payment));
            }
        }
        public List<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }
        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                RaisePropertyChanged(nameof(Customer));
            }
        }
        public Table? Table
        {
            get { return table; }
            set
            {
                table = value;
                RaisePropertyChanged(nameof(Table));
            }
        }
        public Address? Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }

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
