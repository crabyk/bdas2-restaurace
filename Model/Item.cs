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
				 OnPropertyChanged(nameof(Name));
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
				 OnPropertyChanged(nameof(Price));

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
                 OnPropertyChanged(nameof(Image));
            }
        }

        public override object Clone()
        {
            Item item = (Item)this.MemberwiseClone(); 
            // item.Image = Image?.Clone();

            return item;
        }
    }
}
