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
        public ICommand RegisterCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginExecute, CanLoginExecute);
            RegisterCommand = new RelayCommand(RegisterExecute, CanRegisterExecute);
        }

        private void LoginExecute(object obj)
        {

        }

        private bool CanLoginExecute(object obj)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void RegisterExecute(object obj)
        {

        }

        private bool CanRegisterExecute(object obj)
        {
            return true;
        }
    }
}
