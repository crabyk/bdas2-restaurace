using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

    }
}
