using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	public abstract class Item
	{
		private string name;
		private double price;

		public int ID { get; set; }
		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
				RaisePropertyChanged(nameof(Name));
			}
		}

		public double Price
		{
			get
			{
				return price;
			}

			set
			{
				price = value;
				RaisePropertyChanged(nameof(Price));

			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

	}
}
