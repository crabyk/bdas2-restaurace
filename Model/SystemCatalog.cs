using System;

namespace BDAS2_Restaurace.Model
{
    public class SystemCatalog : ModelBase
    {
        private string objectName;
        private string objectType;
        private DateTime created;

        public string ObjectName
        {
            get { return objectName; }
            set
            {
                objectName = value;
                OnPropertyChanged(nameof(objectName));
            }
        }

        public string ObjectType
        {
            get { return objectType; }
            set
            {
                objectType = value;
                OnPropertyChanged(nameof(ObjectType));
            }
        }

        public DateTime Created
        {
            get { return created; }
            set
            {
                created = value;
                OnPropertyChanged(nameof(Created));
            }
        }
    }
}
