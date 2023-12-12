using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class Role : ModelBase
    {
        private string name;

        [Required(ErrorMessage = "Název role je povinný")]
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
