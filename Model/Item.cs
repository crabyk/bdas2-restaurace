using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Model
{
    public abstract class Item : ICloneable
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
				RaisePropertyChanged(nameof(Name));
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
				RaisePropertyChanged(nameof(Price));

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
                RaisePropertyChanged(nameof(Image));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        protected void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

	}
}
