using System;

namespace BDAS2_Restaurace.Model
{
    public class Reservation : ModelBase
    {
        private DateTime reservationDate;
        private int numOfPeople;
        private Customer customer;
        private Table table;

        public DateTime ReservationDate
        {
            get { return reservationDate; }
            set
            {
                reservationDate = value;
                OnPropertyChanged(nameof(ReservationDate));
            }
        }

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
}
