using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class FoodViewModel : ViewModelBase<Food, FoodController>
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

        public ICommand Restore { get; set; }
        public ICommand Cancel { get; set; }

        public FoodViewModel() : base(new FoodController())
        {
            Restore = new RelayCommand(RestoreMethod, CanRestoreMethod);
            Cancel = new RelayCommand(CancelMethod, CanCancelMethod);
            LoadImages();
        }

        private void CancelMethod(object obj)
        {
            new FoodController().Cancel(SelectedItem.ID.ToString());
            Load();
        }

        private bool CanCancelMethod(object arg)
        {
            return PropertyValidateModel.Validate(SelectedItem) && Items.Any(i => i.ID == SelectedItem.ID);
        }

        private void RestoreMethod(object obj)
        {
            new FoodController().Restore(SelectedItem.ID.ToString());
            Load();
        }

        private bool CanRestoreMethod(object arg)
        {
            return PropertyValidateModel.Validate(SelectedItem) && Items.Any(i => i.ID == SelectedItem.ID);
        }

        private void LoadImages()
        {
            Images = new ObservableCollection<ItemImage>(new ItemImageController().GetAll());
        }

        //private async void LoadImages()
        //{
        //    List<ItemImage> result = await Task.Run(() => new ItemImageController().GetAll());
        //    Images = new ObservableCollection<ItemImage>(result);
        //}

        protected override bool IsMatchingFilter(Food item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Name.ToLower().Contains(filterTextLower);
        }
    }
}
