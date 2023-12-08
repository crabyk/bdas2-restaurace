using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BDAS2_Restaurace.ViewModel
{
    public class LoginWindowViewModel : RouteNavigation
    {
        

        public LoginWindowViewModel()
        {
            CurrentViewModel = new LoginViewModel();
            Routes.Add(new Route<LoginViewModel>("login"));
            Routes.Add(new Route<RegisterViewModel>("register"));
        }

    }
}
