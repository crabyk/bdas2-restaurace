using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
    public class WorkShift : ModelBase
    {
        private DateTime begin;
        private DateTime end;

        public WorkShift()
        {
            begin = DateTime.Now;
            end = DateTime.Now;
        }

        public DateTime Begin
        {
            get { return begin; }
            set
            {
                begin = value;
                OnPropertyChanged(nameof(begin));
            }
        }

        public DateTime End
        {
            get { return end; }
            set
            {
                end = value;
                OnPropertyChanged(nameof(end));
            }
        }
    }
}
