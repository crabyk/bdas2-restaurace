using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Errors;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BDAS2_Restaurace.ViewModel
{
    class UserViewModel : ViewModelBase<User, UserController>
    {

        private ObservableCollection<Role> roles;

        private string newPassword;

        public ObservableCollection<Role> Roles
        {
            get { return roles; }
            set
            {
                roles = value;
                OnPropertyChanged(nameof(Roles));
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

        public UserViewModel() : base(new UserController())
        {
            NewPassword = string.Empty;
        }

        private async void LoadRoles()
        {
            List<Role> roles = await Task.Run(() => new RoleController().GetAll());
            Roles = new ObservableCollection<Role>(roles);
        }

        protected override void Load()
        {
            LoadRoles();
            base.Load();
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



                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.None;

                MessageBox.Show("Účet úspěšně registrován", "Registrace", button, icon, MessageBoxResult.OK);
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

        /*
        protected override void CreateMethod(object obj)
        {
            ErrorHandler.OpenDialog(ErrorType.Create);
            try
            {
                controller.Register(SelectedItem, NewPassword);
            }
            catch (Exception ex)
            {
                ErrorHandler.OpenDialog(ErrorType.Create);
            }

            NewPassword = string.Empty;
            Load();
        }
        */


        protected override void UpdateMethod(object obj)
        {

            try
            {
                controller.Update(SelectedItem, NewPassword);
            }
            catch (Exception ex)
            {
                ErrorHandler.OpenDialog(ErrorType.Update);
            }
            NewPassword = string.Empty;
            Load();
        }
    }
}
