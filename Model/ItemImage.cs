using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Model
{
    public class ItemImage : ModelBase
    {
        private string fileName;
        private DateTime modifyDate;
        private BitmapImage image;

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public DateTime ModifyDate
        {
            get { return modifyDate; }
            set
            {
                modifyDate = value;
                OnPropertyChanged(nameof(ModifyDate));
            }
        }

        public BitmapImage Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }


        public override object Clone()
        {
            ItemImage itemImage = (ItemImage)MemberwiseClone();

            return base.Clone();
        }
    }
}
