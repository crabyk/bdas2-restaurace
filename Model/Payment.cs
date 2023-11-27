using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
    public class Payment : BindableBase
    {
        private double amount;
        private DateTime date;
        private PaymentType type;

        public int ID { get; set; } 
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
