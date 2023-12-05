using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Customer : Person
    {
        private Address address;
        public Address? Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChanged(nameof(Address));
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

        public override object Clone()
        {
            Customer customer = (Customer)this.MemberwiseClone();
            customer.Address = (Address)Address?.Clone();

            return customer;
        }
    }
}
