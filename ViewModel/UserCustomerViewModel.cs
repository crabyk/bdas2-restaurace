using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BDAS2_Restaurace.ViewModel
{
    public class UserCustomerViewModel : RouteNavigation
    {
        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }


        public ICommand LoadOrders { get; set; }
        public ICommand LoadReservations { get; set; }

        public UserCustomerViewModel()
        {
            LoadOrders = new RelayCommand(LoadOrdersMethod, CanLoadOrdersMethod);
        }

        public UserCustomerViewModel(Customer customer)
        {
            Customer = customer;
            CurrentViewModel = new UserOrderViewModel(customer);
        }

        private bool CanLoadOrdersMethod(object arg)
        {
            return true;
        }

        private void LoadOrdersMethod(object obj)
        {
            CurrentViewModel = new UserOrderViewModel(Customer);
        }
    }
}
