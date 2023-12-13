using System;
using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class Reservation : ModelBase
    {
        private DateTime reservationDate;
        private int numOfPeople;
        private Customer customer;
        private Table table;

        [ReservationDate(ErrorMessage = "Neplatné datum rezervace")]
        public DateTime ReservationDate
        {
            get { return reservationDate; }
            set
            {
                reservationDate = value;
                OnPropertyChanged(nameof(ReservationDate));
            }
        }

        [Required(ErrorMessage = "Počet lidí je povinný"), Range(1, int.MaxValue, ErrorMessage = "Počet lidí musí být 1 a více")]
        public int NumberOfPeople
        {
            get { return numOfPeople; }
            set
            {
                numOfPeople = value;
                OnPropertyChanged(nameof(NumberOfPeople));
            }
        }

        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        public Table Table
        {
            get { return table; }
            set
            {
                table = value;
                OnPropertyChanged(nameof(Table));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ReservationDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Today;
            }

            return false;
        }
    }
}
