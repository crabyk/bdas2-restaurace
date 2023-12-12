using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class LogViewModel : ViewModelBase<Log, LogController>
    {
        public LogViewModel() : base(new LogController())
        {
        }

        protected override bool IsMatchingFilter(Log item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return
                item.TableName.ToLower().Contains(filterTextLower) ||
                item.ActionType.ToLower().Contains(filterTextLower);
        }
    }
}
