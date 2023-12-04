using System;
using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Payment : ModelBase
    {
        private double amount;
        private DateTime date;
        private PaymentType type;

        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        public PaymentType Type
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChanged(nameof(Type));
            }
        }
    }
}
