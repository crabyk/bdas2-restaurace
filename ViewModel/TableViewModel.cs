using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class TableViewModel : ViewModelBase<Table, TableController>
    {
        public TableViewModel() : base(new TableController())
        {
        }

        protected override bool IsMatchingFilter(Table item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Number.ToString().ToLower().Contains(filterTextLower);
        }
    }
}
