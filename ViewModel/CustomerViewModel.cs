using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
        }

        private async void LoadAddresses()
        {
            List<Address> addresses = await Task.Run(() => new AddressController().GetAll());
            Addresses = new ObservableCollection<Address>(addresses);
        }

        protected override void Load()
        {
            LoadAddresses();
            base.Load();
        }

        protected override bool IsMatchingFilter(Customer item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return
                item.FirstName.ToLower().Contains(filterTextLower) ||
                item.LastName.ToLower().Contains(filterTextLower) ||
                item.Email.ToLower().Contains(filterTextLower) ||
                (item.User != null && item.User.Login.ToLower().Contains(filterTextLower));
        }
    }
}
