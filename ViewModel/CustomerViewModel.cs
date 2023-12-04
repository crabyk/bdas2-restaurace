using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class CustomerViewModel : ViewModelBase<Customer, CustomerController>
    {

        public CustomerViewModel() : base(new CustomerController())
        {
        }


    }
}
