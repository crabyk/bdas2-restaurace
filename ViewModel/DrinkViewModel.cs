using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;

namespace BDAS2_Restaurace.ViewModel
{
    public class DrinkViewModel : ViewModelBase<Drink, DrinkController>
    {
        private ObservableCollection<ItemImage> images;


        public ObservableCollection<ItemImage> Images
        {
            get { return images; }
            set
            {
                images = value;
                OnPropertyChanged(nameof(Images));
            }
        }

        public DrinkViewModel() : base(new DrinkController())
        {
            LoadImages();
        }

        private void LoadImages()
        {
            Images = new ObservableCollection<ItemImage>(new ItemImageController().GetAll());
        }

        protected override bool IsMatchingFilter(Drink item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Name.ToLower().Contains(filterTextLower);
        }
    }
}
