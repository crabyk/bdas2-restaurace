﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BDAS2_Restaurace.Model
{
    public class Reservation : ModelBase
    {
        private DateTime reservationDate;
        private int numOfPeople;
        private Customer customer;
        private Table table;

        public Reservation()
        {
            ReservationDate = DateTime.Now;
            NumberOfPeople = 0;
            Customer = new Customer();
            Table = new Table();    
        }

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

        [Range(1, 16, ErrorMessage = "Počet osob musí být mezi 1 a 16")]
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

        [Required(ErrorMessage ="Stůl je povinný")]
        public Table Table
        {
            get { return table; }
            set
            {
                table = value;
                OnPropertyChanged(nameof(Table));
            }
        }
        public override object Clone()
        {
            Reservation order = (Reservation)this.MemberwiseClone();
            order.Table = (Table)Table?.Clone();
            order.Customer = (Customer)Customer?.Clone();

            return order;
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
