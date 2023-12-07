namespace BDAS2_Restaurace.Model
{
    public class User : ModelBase
    {
        private string login;
        private string hash;
        private string firstName;
        private string lastName;
        private Role role;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Hash
        {
            get { return hash; }
            set
            {
                hash = value;
                OnPropertyChanged(nameof(Hash));
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public Role Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
    }
}
