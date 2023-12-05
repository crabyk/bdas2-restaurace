using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class CustomerViewModel : ViewModelBase<Customer, CustomerController>
    {
        private ObservableCollection<Address> addresses;


        public ObservableCollection<Address> Addresses
        {
            get { return addresses; }
            set
            {
                addresses = value;
                OnPropertyChanged(nameof(Addresses));
            }
        }


        public CustomerViewModel() : base(new CustomerController())
        {
            Addresses = new ObservableCollection<Address>(new AddressController().GetAll());
        }


    }
}
