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
    public class UserReservationViewModel : ViewModelBase<Reservation, ReservationController>
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

        public UserReservationViewModel(Customer customer) : base(new ReservationController())
        {
            SelectedCustomer = customer;
            List<Reservation> result = controller.GetAll(customer);
            ObservableCollection<Reservation> items = new ObservableCollection<Reservation>(result);
            Items = items;
            if (items.Count() > 0)
                SelectedItem = items.First();

        }

        protected override void Load()
        {
        }
    }
}
