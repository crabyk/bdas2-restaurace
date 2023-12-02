using System;
using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public abstract class Person
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private DateTime birthDate;
        private string email;


		public int ID { get; set; }
		public string FirstName
		{
			get { return firstName; }
			set
			{
				firstName = value;
				RaisePropertyChanged(nameof(FirstName));
			}
		}
		public string LastName
		{
			get { return lastName; }
			set
			{
				lastName = value;
				RaisePropertyChanged(nameof(LastName));
			}
		}
		public string FullName
		{
			get
			{
				return FirstName + " " + LastName;
			}
		}
		public DateTime BirthDate
		{
			get { return  birthDate; }
			set
			{
				birthDate = value;
				RaisePropertyChanged(nameof(BirthDate));
			}
		}
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set
			{
				phoneNumber = value;
				RaisePropertyChanged(nameof(PhoneNumber));
			}
		}
		}

        public string Email
        {
            get { return email; }
            set
            {
                /*
				if (!Regex.Match(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").Success)
					throw new ArgumentException("Špatný formát pro email", nameof(email));
				*/
                email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        public int Age()
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - BirthDate.Year;
            if (BirthDate > currentDate.AddYears(-age))
                age--;

            return age;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
