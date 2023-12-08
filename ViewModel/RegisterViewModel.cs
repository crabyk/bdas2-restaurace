using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;

namespace BDAS2_Restaurace.ViewModel
{
    public class RegisterViewModel : ViewModelBase<User, UserController>
    {
        private string newPassword;

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
            User newUser = new User();
            newUser.Role = new RoleController().Get("3");
            SelectedItem = newUser;
            NewPassword = string.Empty;
        }

        protected override void CreateMethod(object obj)
        {
            new UserController().Register(SelectedItem, NewPassword);
            NewPassword = string.Empty;
            // base.CreateMethod(obj);
            // TODO vratit se zpet na domovskou stranku
        }
    }
}
