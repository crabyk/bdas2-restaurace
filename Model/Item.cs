using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Model
{
    public abstract class Item : ModelBase
    {
        private string name;
        private double price;
        private BitmapImage? image;

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
	}
}
