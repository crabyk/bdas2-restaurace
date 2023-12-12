using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class SystemCatalogViewModel : ViewModelBase<SystemCatalog, SystemCatalogController>
    {
        public SystemCatalogViewModel() : base(new SystemCatalogController())
        {
        }

        protected override bool IsMatchingFilter(SystemCatalog item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return
                item.ObjectName.ToLower().Contains(filterTextLower) ||
                item.ObjectType.ToLower().Contains(filterTextLower);
        }
    }
}
