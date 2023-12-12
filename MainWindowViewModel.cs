using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using BDAS2_Restaurace.ViewModel;
using BDAS2_Restaurace.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace
{
	class MainWindowViewModel : RouteNavigation
	{
		public ICommand OpenAdmin { get; set; }

		public MainWindowViewModel() : base()
		{
			OpenAdmin = new RelayCommand(OpenAdminMethod, CanOpenAdminMethod);
			Routes.Add(new Route<MenuViewModel>("menu"));
			Routes.Add(new Route<OrderViewModel>("order"));
			Routes.Add(new Route<ReservationViewModel>("reservation"));
			Routes.Add(new Route<AdminViewModel>("admin"));
            Routes.Add(new Route<LoginViewModel>("login"));
            Routes.Add(new Route<RegisterViewModel>("register"));

        }

        private bool CanOpenAdminMethod(object obj)
        {
			return true;
        }

        private void OpenAdminMethod(object obj)
        {
			var win = new LoginWindow();
			win.Show();
        }

        /*
		public MainWindowViewModel()
		{
			NavCommand = new MyICommand<string>(OnNav);
		}

		private MenuViewModel menuViewModel = new MenuViewModel();

		private BindableBase CurrentViewModel;

		public BindableBase _CurrentViewModel
		{
			get { return CurrentViewModel; }
			set { SetProperty(ref CurrentViewModel, value); }
		}

		public MyICommand<string> NavCommand { get; private set; }

		private void OnNav(string destination)
		{

			switch (destination)
			{
				case "menu":
					CurrentViewModel = menuViewModel;
					break;
				default:
					CurrentViewModel = null;
					break;
			}
		}
		*/
    }
}
