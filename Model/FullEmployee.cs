using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
    public class FullEmployee : Employee
    {
        private float monthRate;

        public FullEmployee() : base()
        {
            employmentType = "Plny uvazek";
            monthRate = 0;
        }

        public float MonthRate
        {
            get { return monthRate; }
            set
            {
                monthRate = value;
                OnPropertyChanged(nameof(MonthRate));
            }
        }
    }
}
