using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BDAS2_Restaurace.ViewModel
{
    public class EmulationViewModel : RouteNavigation
    {

        private ObservableCollection<User> customers;

        private User selectedCustomer;

        private string emulationName = string.Empty;

        public ObservableCollection<User> Customers
        {
            get { return customers; }
            set
            {
                customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public User SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }


        public string EmulationName
        {
            get { return emulationName; }
            set
            {
                emulationName = value;
                OnPropertyChanged(nameof(EmulationName));
            }
        }

        public ICommand EmulateCustomer { get; set; }

        public EmulationViewModel()
        {
            // EmulationName = "Emulace pracovníka";
            CurrentViewModel = new AdminViewModel();
            Routes.Add(new Route<AdminViewModel>("admin"));
            Routes.Add(new Route<CustomerViewModel>("customer"));
            Routes.Add(new Route<EmployeeViewModel>("employee"));

            EmulateCustomer = new RelayCommand(EmulateCustomerMethod, CanEmulateCustomerMethod);
            Load();
        }

        private async void LoadCustomers()
        {
            List<User> customers = await Task.Run(() => new UserController().GetAll());

            customers = customers.Where(c => c.Role.ID  == 2).ToList();

            Customers = new ObservableCollection<User>(customers);
            SelectedCustomer = Customers.First();

        }

        private async void Load()
        {
            LoadCustomers();
        }

        private bool CanEmulateCustomerMethod(object arg)
        {
            return true;
        }

        private void EmulateCustomerMethod(object obj)
        {
            Customer? customer = new CustomerController().GetByUser(SelectedCustomer);

            if (customer != null)
            {

                CurrentViewModel = new UserCustomerViewModel(customer);
            }
        }
    }
}
