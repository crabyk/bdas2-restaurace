using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ICommand Restore { get; set; }
        public ICommand Cancel { get; set; }

        public DrinkViewModel() : base(new DrinkController())
        {
            Restore = new RelayCommand(RestoreMethod, CanRestoreMethod);
            Cancel = new RelayCommand(CancelMethod, CanCancelMethod);
            LoadImages();
        }

        private void CancelMethod(object obj)
        {
            new DrinkController().Cancel(SelectedItem.ID.ToString());
            Load();
        }

        private bool CanCancelMethod(object arg)
        {
            return PropertyValidateModel.Validate(SelectedItem) && Items.Any(i => i.ID == SelectedItem.ID);
        }

        private void RestoreMethod(object obj)
        {
            new DrinkController().Restore(SelectedItem.ID.ToString());
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

        protected override bool IsMatchingFilter(Drink item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Name.ToLower().Contains(filterTextLower);
        }
    }
}
