using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.ViewModel
{
    public class ItemImageViewModel : ViewModelBase<ItemImage, ItemImageController>
    {
        public ICommand UploadImage { get; set; }

        public ItemImageViewModel() : base(new ItemImageController())
        {
            UploadImage = new RelayCommand(UploadImageMethod, CanUploadImageMethod);
        }

        private bool CanUploadImageMethod(object obj)
        {
            return true;
        }

        private void UploadImageMethod(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";
            fileDialog.Title = "Select an Image File";

            if (fileDialog.ShowDialog() == true)
            {
                string filePath = fileDialog.FileName;

                try
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(filePath));

                    SelectedItem = new ItemImage
                    {
                        FileName = Path.GetFileName(filePath),
                        ModifyDate = File.GetLastWriteTime(filePath),
                        Image = bitmapImage
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected override void Load()
        {
            Items = new ObservableCollection<ItemImage>(controller.GetAll());
            // base.Load();
        }

        protected override bool IsMatchingFilter(ItemImage item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.FileName.ToLower().Contains(filterTextLower);
        }
    }
}
