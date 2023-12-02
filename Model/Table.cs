using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Table
    {
        private int number;

        public int ID { get; set; }
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
