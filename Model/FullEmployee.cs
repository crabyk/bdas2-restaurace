using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            monthRate = 0;
        }

        [Range(18000, 60000, ErrorMessage = "Hodnota musí být od 18000Kč do 60000Kč")]
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
