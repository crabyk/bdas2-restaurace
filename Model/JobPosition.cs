using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
    public class JobPosition : ModelBase
    {
        private string name;

        public JobPosition()
        {
            name = string.Empty; 
        }

        [Required(ErrorMessage = "Název pozice je povinný")]
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
