using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using BDAS2_Restaurace.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace
{
	class MainWindowViewModel : RouteNavigation
	{
		public MainWindowViewModel() : base()
		{
			this.Routes.Add(new Route<MenuViewModel>("menu"));
			this.Routes.Add(new Route<OldOrderViewModel>("order"));
			this.Routes.Add(new Route<AdminViewModel>("admin"));
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
