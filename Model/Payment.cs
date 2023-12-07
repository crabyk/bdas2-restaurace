using System;
using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Payment : ModelBase
    {
        private double amount;
        private DateTime date;
        private PaymentType type;

        public Payment()
        {
            amount = 0;
            date = DateTime.Now;
            type = new PaymentType();
        }

        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public PaymentType Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }


        public override object Clone()
        {
            Payment payment = (Payment)this.MemberwiseClone();
            payment.Type = (PaymentType)Type?.Clone();
            return payment;
        }


    }
}
