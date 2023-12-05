using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
	public class AdminViewModel : RouteNavigation
	{
		public AdminViewModel()
		{

			this.Routes.Add(new Route<DrinkViewModel>("drink"));
			this.Routes.Add(new Route<FoodViewModel>("food"));
			this.Routes.Add(new Route<OrderViewModel>("order"));
            this.Routes.Add(new Route<PaymentTypeViewModel>("paymentType"));
            this.Routes.Add(new Route<CustomerViewModel>("customer"));
            this.Routes.Add(new Route<AddressViewModel>("address"));

        }
	}
}
