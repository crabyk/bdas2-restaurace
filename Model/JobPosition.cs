using System;
using System.Collections.Generic;
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
