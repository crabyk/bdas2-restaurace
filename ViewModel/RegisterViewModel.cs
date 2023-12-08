using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;

namespace BDAS2_Restaurace.ViewModel
{
    public class RegisterViewModel : ViewModelBase<User, UserController>
    {
        private Customer newCustomer;
        private Address newAddress;
        private string newPassword;

        public Customer NewCustomer
        {
            get { return newCustomer; }
            set
            {
                newCustomer = value;
                OnPropertyChanged(nameof(NewCustomer));
            }
        }

        public Address NewAddress
        {
            get { return newAddress; }
            set
            {
                newAddress = value;
                OnPropertyChanged(nameof(NewAddress));
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        public RegisterViewModel() : base(new UserController())
        {
            NewAddress = new Address();
            NewCustomer = new Customer();
            User newUser = new User();
            newUser.Role = new RoleController().Get("2");
            SelectedItem = newUser;
            NewPassword = string.Empty;
        }

        protected override void CreateMethod(object obj)
        {
            User newUser = new UserController().Register(SelectedItem, NewPassword);
            NewCustomer.User = newUser;
            NewCustomer.FirstName = newUser.FirstName;
            NewCustomer.LastName = newUser.LastName;

            Address newAddress = new AddressController().Add(NewAddress);
            NewCustomer.Address = newAddress;

            new CustomerController().Add(NewCustomer);
            NewPassword = string.Empty;
            // base.CreateMethod(obj);
            // TODO vratit se zpet na domovskou stranku
        }
    }
}
