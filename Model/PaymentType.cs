using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class PaymentType : ModelBase
    {
        private string name;

        public PaymentType()
        {
            name = string.Empty;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                 OnPropertyChanged(nameof(Name));
            }
        }

    }
}
