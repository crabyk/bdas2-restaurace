using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class RoleViewModel : ViewModelBase<Role, RoleController>
    {
        public RoleViewModel() : base(new RoleController())
        {
        }

        protected override bool IsMatchingFilter(Role item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Name.ToLower().Contains(filterTextLower);
        }
    }
}
