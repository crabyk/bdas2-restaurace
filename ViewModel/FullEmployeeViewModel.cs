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
    public class FullEmployeeViewModel : ViewModelBase<FullEmployee, FullEmployeeController>
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

        public FullEmployeeViewModel() : base(new FullEmployeeController())
        {

        }

        async void LoadAddresses()
        {
            List<Address> addresses = await Task.Run(() => new AddressController().GetAll());
            Addresses = new ObservableCollection<Address>(addresses);
        }

        protected override void Load()
        {
            LoadAddresses();
            base.Load();
        }
    }
}
