using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class User : ModelBase
    {
        private string login;
        private string password;
        private string firstName;
        private string lastName;
        private Role role;

        [Required(ErrorMessage = "Login je povinný")]
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
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

        [Required(ErrorMessage = "Jméno je povinné")]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        [Required(ErrorMessage = "Příjmení je povinné")]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        [Required(ErrorMessage = "Role je povinná")]
        public Role Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public override object Clone()
        {
            User user = (User)this.MemberwiseClone();
            user.Role = (Role)Role?.Clone();

            return user;
        }
    }
}
