using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Table : ModelBase
    {
        private int number;

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                RaisePropertyChanged(nameof(Number));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
