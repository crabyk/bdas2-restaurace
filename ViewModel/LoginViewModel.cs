using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using BDAS2_Restaurace.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class LoginViewModel : BindableBase
    {


        private string username = string.Empty;
        private string password = string.Empty;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            Username = string.Empty;
            Password = string.Empty;
            LoginCommand = new RelayCommand(LoginMethod, CanLoginMethod);
            Load();
        }

        private async void LoadRoles()
        {
            List<Role> roles = await Task.Run(() => new RoleController().GetAll());
        }

        private void Load()
        {
            LoadRoles();
        }

        private bool CanLoginMethod(object obj)
        {
            // return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
            return true;
        }

        private void LoginMethod(object obj)
        {

            try
            {
                Window window;
                User? user = new UserController().Login(Username, Password);
                if (user == null)
                    return;


                switch (user.Role.ID)
                {
                    case 1: // Admin
                        window = new AdminWindow();
                        break;
                    case 2: // Zakaznik
                        Customer customer = new CustomerController().GetByUser(user);
                        window = new CustomerWindow(customer);
                        break;
                    case 3: // Zamestnanec
                        window = new EmployeeWindow();
                        break;
                    default:
                        return;
                }

                Username = string.Empty;
                Password = string.Empty;

                window.Show();
                // window = new AdminWindow();
            }
            catch (Exception ex) { return; }



            // OnWindowClose();

            /*
            if (Password == "admin" && Username == "admin")
            {
                var win = new AdminWindow();
                win.Show();
                
            }
            */
        }
    }
}
