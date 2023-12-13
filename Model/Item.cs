using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public abstract class Item : ModelBase
    {
        private string name;
        private double price;
        private ItemImage? itemImage;
        private int available = 1;
        private int totalOrders;

        [Required(ErrorMessage = "Název je povinný")]
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

        [Required(ErrorMessage = "Obrázek je povinný")]
        public ItemImage? ItemImage
        {
            get
            {
                return itemImage;
            }
            set
            {
                itemImage = value;
                OnPropertyChanged(nameof(ItemImage));
            }
        }

        public int Available
        {
            get { return available; }
            set
            {
                available = value;
                OnPropertyChanged(nameof(Available));
            }
        }

        public int TotalOrders
        {
            get { return totalOrders; }
            set
            {
                totalOrders = value;
                OnPropertyChanged(nameof(TotalOrders));
            }
        }

        public override object Clone()
        {
            Item item = (Item)this.MemberwiseClone();
            item.ItemImage = (ItemImage)ItemImage?.Clone();

            return item;
        }
    }
}
