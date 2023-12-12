using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
	public class AdminViewModel : RouteNavigation
	{
        public ICommand EmulateEmployee { get; set; }
		public AdminViewModel()
		{
			Routes.Add(new Route<DrinkViewModel>("drink"));
			Routes.Add(new Route<FoodViewModel>("food"));
            Routes.Add(new Route<ItemImageViewModel>("itemImage"));
            Routes.Add(new Route<OrderViewModel>("order"));
            Routes.Add(new Route<PaymentTypeViewModel>("paymentType"));
            Routes.Add(new Route<CustomerViewModel>("customer"));
            Routes.Add(new Route<AddressViewModel>("address"));
            Routes.Add(new Route<TableViewModel>("table"));
            Routes.Add(new Route<FullEmployeeViewModel>("fullEmployee"));
            Routes.Add(new Route<PartEmployeeViewModel>("partEmployee"));
            Routes.Add(new Route<WorkShiftViewModel>("shift"));
            Routes.Add(new Route<JobPositionViewModel>("position"));
            Routes.Add(new Route<UserViewModel>("user"));
            Routes.Add(new Route<RoleViewModel>("role"));
            Routes.Add(new Route<LogViewModel>("logs"));
            Routes.Add(new Route<SystemCatalogViewModel>("catalog"));


            EmulateEmployee = new RelayCommand(EmulateEmployeeMethod, CanEmulateEmployeeMethod);
        }

        private bool CanEmulateEmployeeMethod(object arg)
        {
            return true;
        }

        private void EmulateEmployeeMethod(object obj)
        {
            CurrentViewModel = new LoginViewModel();
        }
    }
}
