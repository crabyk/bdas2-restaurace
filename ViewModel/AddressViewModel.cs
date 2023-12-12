using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class AddressViewModel : ViewModelBase<Address, AddressController>
    {
        public AddressViewModel() : base(new AddressController())
        {
        }

        protected override bool IsMatchingFilter(Address item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return
                item.StreetName.ToLower().Contains(filterTextLower) ||
                item.CityName.ToLower().Contains(filterTextLower) ||
                item.UnitNumber.ToLower().Contains(filterTextLower) ||
                item.PostalCode.ToLower().Contains(filterTextLower) ||
                item.Country.ToLower().Contains(filterTextLower);
        }
    }
}
