using BDAS2_Restaurace.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Router
{
	public class RouteNavigation : BindableBase
	{
		private List<Route> routes = new List<Route>();

        public List<Route> Routes { get { return routes; } }	

		public RouteNavigation()
		{
			NavCommand = new MyICommand<string>(OnNav);
		}


		private BindableBase _CurrentViewModel;

		public BindableBase CurrentViewModel
		{
			get { return _CurrentViewModel; }
			set { SetProperty(ref _CurrentViewModel, value); }
		}

		public MyICommand<string> NavCommand { get; private set; }

		private void OnNav(string destination)
		{
			Route route = routes.Find(r => destination == r.Name);

			if (route == null)
				return;
			/*
			Type viewModelType = route.ViewModel.GetType();
			BindableBase? newInstance = (BindableBase?)Activator.CreateInstance(viewModelType);
			*/
			BindableBase? newInstance = route.NewViewModel();
			newInstance.WindowClose += (s, e) =>
			{
				CurrentViewModel = null;
			};

			newInstance.WindowChange += (b) =>
			{
				CurrentViewModel = b;
			};

            CurrentViewModel = newInstance ?? route.ViewModel;
		}

    }
}
