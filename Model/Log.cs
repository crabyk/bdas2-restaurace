using System;

namespace BDAS2_Restaurace.Model
{
    public class Log : ModelBase
    {
        private string tableName;
        private int affectedId;
        private string actionType;
        private DateTime time;

        public string TableName
        {
            get { return tableName; }
            set
            {
                tableName = value;
                OnPropertyChanged(nameof(TableName));
            }
        }

        public int AffectedId
        {
            get { return affectedId; }
            set
            {
                affectedId = value;
                OnPropertyChanged(nameof(AffectedId));
            }
        }

        public string ActionType
        {
            get { return actionType; }
            set
            {
                actionType = value;
                OnPropertyChanged(nameof(ActionType));
            }
        }

        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
    }
}
