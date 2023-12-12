using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class PaymentType : ModelBase
    {
        private string name;

        public PaymentType()
        {
            name = string.Empty;
        }

        [Required(ErrorMessage ="Nazev typu platby je povinný")]
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
