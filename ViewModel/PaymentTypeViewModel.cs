using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class PaymentTypeViewModel : ViewModelBase<PaymentType, PaymentTypeController>
    {
        public PaymentTypeViewModel() : base(new PaymentTypeController())
        {
        }

        protected override bool IsMatchingFilter(PaymentType item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Name.ToLower().Contains(filterTextLower);
        }
    }
}
