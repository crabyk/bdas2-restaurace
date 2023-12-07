using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Table : ModelBase
    {
        private int number;

        public Table()
        {
            number = 0;
        }

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

    }
}
