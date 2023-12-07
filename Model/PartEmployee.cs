using System;
using System.Collections.Generic;
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
