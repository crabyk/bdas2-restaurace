using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BDAS2_Restaurace.Model
{
    public class Customer : Person
    {
        private DateTime birthDate;
        private string phoneNumber;
        private string email;
        private User? user;

        public Customer() : base()
        {
            birthDate = DateTime.Now;
            phoneNumber = string.Empty;
            email = string.Empty;
        }

        public User? User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }


        [Required(ErrorMessage = "Datum narození je povinné")]
        [DataType(DataType.Date, ErrorMessage = "Špatný formát datumu")]
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        [Required(ErrorMessage = "Telefon je povinný")]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        [Required(ErrorMessage = "Email je povinný")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Špatný formát emailu")]
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
                OnPropertyChanged(nameof(Email));
            }
        }

        public override object Clone()
        {
            Customer customer = (Customer)this.MemberwiseClone();
            customer.Address = (Address)Address?.Clone();

            return customer;
        }
    }
}
