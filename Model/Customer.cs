using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	public class Customer : Person
	{
		private Address address;
		public Address? Address
		{
			get { return address; }
			set
			{
				address = value;
				RaisePropertyChanged(nameof(Address));
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
