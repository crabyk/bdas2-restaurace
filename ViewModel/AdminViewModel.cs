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
			this.Routes.Add(new Route("drink", new DrinkViewModel()));
			this.Routes.Add(new Route("food", new FoodViewModel()));
			this.Routes.Add(new Route("order", new OrderViewModel()));
		}
	}
}
