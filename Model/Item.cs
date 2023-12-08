namespace BDAS2_Restaurace.Model
{
    public abstract class Item : ModelBase
    {
        private string name;
        private double price;
        private ItemImage? itemImage;

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

        public override object Clone()
        {
            Item item = (Item)this.MemberwiseClone();
            item.ItemImage = (ItemImage)ItemImage?.Clone();

            return item;
        }
    }
}
