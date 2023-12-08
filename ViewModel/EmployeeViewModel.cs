using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
    class EmployeeViewModel : RouteNavigation
    {
        public EmployeeViewModel()
        {
            this.Routes.Add(new Route<OrderViewModel>("order"));
            this.Routes.Add(new Route<DrinkViewModel>("drink"));
            this.Routes.Add(new Route<FoodViewModel>("food"));
        }
    }
}
