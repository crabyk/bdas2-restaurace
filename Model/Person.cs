using System;
using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public abstract class Person : ModelBase
    {
        private string firstName;
        private string lastName;
        private Address address;


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
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

    }
}
