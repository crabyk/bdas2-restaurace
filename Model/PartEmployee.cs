using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
    public class PartEmployee : Employee
    {
        private float hourRate;

        public PartEmployee() : base()
        {
            hourRate = 0;
        }

        [Range(100, 1000, ErrorMessage = "Hodnota musí být od 100Kč do 1000Kč")]
        public float HourRate
        {
            get { return hourRate; }
            set
            {
                hourRate = value;
                OnPropertyChanged(nameof(HourRate));
            }
        }
    }
}
