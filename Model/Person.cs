using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public abstract class Person : ModelBase
    {
        private string firstName;
        private string lastName;
        private Address address;

        [Required(ErrorMessage = "Adresa je povinná")]
        public Address? Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        protected Person()
        {
            firstName = string.Empty;
            lastName = string.Empty;
            address = new Address();
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
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

    }
}
