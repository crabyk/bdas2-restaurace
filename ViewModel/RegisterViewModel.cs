using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Errors;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

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

        protected override bool CanCreateMethod(object obj)
        {
            return base.CanCreateMethod(obj) && !string.IsNullOrWhiteSpace(SelectedItem.Password);
        }

        protected override void CreateMethod(object obj)
        {
            if (new UserController().Find(SelectedItem.Login))
            {
                ErrorHandler.OpenDialog($"Uživatel {SelectedItem.Login} již existuje", "Registrace");
                return;
            }

            try
            {
                User newUser = new UserController().Register(SelectedItem, SelectedItem.Password);
                NewCustomer.User = newUser;
                NewCustomer.FirstName = newUser.FirstName;
                NewCustomer.LastName = newUser.LastName;

                Address newAddress = new AddressController().Add(NewAddress);
                NewCustomer.Address = newAddress;

                new CustomerController().Add(NewCustomer);

                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.None;

                MessageBox.Show("Zákazník úspěšně registrován", "Registrace", button, icon, MessageBoxResult.OK);
            }
            catch (Exception ex)
            {
                // ErrorHandler.OpenDialog(ErrorType.Create);
            }
            SelectedItem.Password = string.Empty;
            OnWindowChange(new LoginViewModel());
            // base.CreateMethod(obj);
            // TODO vratit se zpet na domovskou stranku
        }
    }
}
