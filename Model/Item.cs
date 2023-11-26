using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Model
{
    public abstract class Item
    {
        private string name;
        private double price;
        private BitmapImage? image;

        public int ID { get; set; }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
                RaisePropertyChanged("Price");

            }
        }

        public BitmapImage? Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                RaisePropertyChanged("Image");
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
