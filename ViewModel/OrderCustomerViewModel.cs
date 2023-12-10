using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace BDAS2_Restaurace.ViewModel
{
    public class OrderCustomerViewModel : ViewModelBase<Order, OrderController>
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

        public OrderCustomerViewModel(Customer customer) : base(new OrderController())
        {
            SelectedCustomer = customer;
        }

        protected override async void Load()
        {
            List<Order> result = await Task.Run(() => controller.GetAll(SelectedCustomer));
            ObservableCollection<Order> items = new ObservableCollection<Order>(result);
            Items = items;
        }
    }
}
