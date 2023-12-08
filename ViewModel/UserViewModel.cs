﻿using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged(nameof(newPassword));
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


        protected override void CreateMethod(object obj)
        {
            new UserController().Register(SelectedItem, NewPassword);
            NewPassword = string.Empty;
            Load();
        }


        protected override void UpdateMethod(object obj)
        {
            new UserController().Update(SelectedItem, NewPassword);
            NewPassword = string.Empty;
            Load();
        }
    }
}
