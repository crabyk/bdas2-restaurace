using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
    public class UserOrderViewModel : ViewModelBase<Order, OrderController>
    {
        private Customer selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        public UserOrderViewModel(Customer customer) : base(new OrderController())
        {
            SelectedCustomer = customer;
            List<Order> result = controller.GetAll(customer);
            ObservableCollection<Order> items = new ObservableCollection<Order>(result);
            Items = items;
        }

        protected override void Load()
        {
        }
    }
}
