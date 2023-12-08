using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using BDAS2_Restaurace.Windows;
using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        private string username;
        private string password;

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
                window = new AdminWindow();
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
