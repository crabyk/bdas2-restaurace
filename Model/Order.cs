using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Order : ModelBase
    {
        private DateTime orderDate;
        private Payment payment;
        private List<Item> items = new List<Item>();
        private Customer customer;
        private Table? table;
        private Address? address;

        public DateTime OrderDate
        {
            get { return orderDate; }
            set
            {
                orderDate = value;
                 OnPropertyChanged(nameof(OrderDate));
            }
        }
        public Payment Payment
        {
            get { return payment; }
            set
            {
                payment = value;
                OnPropertyChanged(nameof(Payment));
            }
        }
        public List<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                 OnPropertyChanged(nameof(Customer));
            }
        }
        public Table? Table
        {
            get { return table; }
            set
            {
                table = value;
                 OnPropertyChanged(nameof(Table));
            }
        }
        public Address? Address
        {
            get { return address; }
            set
            {
                address = value;
                 OnPropertyChanged(nameof(Address));
            }
        }


        public override object Clone()
        {
            Order order = (Order)this.MemberwiseClone();
            order.Payment = (Payment)Payment?.Clone();
            order.Address = (Address)Address?.Clone();
            order.Table = (Table)Table?.Clone();
            order.Customer = (Customer)Customer?.Clone();

            /*
            order.Items = new List<Item>();
            foreach (var item in Items)
                order.Items.Add((Item)item.Clone());
            */
            

            return order;
        }


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
    }
}
