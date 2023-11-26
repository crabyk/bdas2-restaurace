using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;

namespace BDAS2_Restaurace.ViewModel
{
	public class CustomerViewModel : BindableBase
	{
		public ICommand AddUser { get; set; }

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;

		// public Customer Customer { get; set; }
		public ObservableCollection<Customer> Customers
		{
			get;
			set;
		}

		public Customer NewCustomer { get; set; }


		public CustomerViewModel()
		{
			NewCustomer = new Customer();
			Customers = new ObservableCollection<Customer>();
			AddUser = new RelayCommand(Add, CanAdd);

			Load();
		}

		public void Load()
		{
			Customers = new ObservableCollection<Customer>
			{
				new Customer { FirstName = "John", LastName = "Scena" }
			};
		}

		public bool CanAdd(object obj)
		{
			return true;
		}

		public void Add(object obj)
		{

			/*
			Customer newCustomer = new Customer
			{
				FirstName = NewCustomer.FirstName,
				LastName = NewCustomer.LastName
			};	
			*/
			
			Customers.Add(new Customer()
			{
				FirstName = NewCustomer.FirstName,
				LastName = NewCustomer.LastName,
				BirthDate = NewCustomer.BirthDate,
				PhoneNumber = NewCustomer.PhoneNumber,
				Email = NewCustomer.Email,
			});	

			// Load();
		}
	}
}
