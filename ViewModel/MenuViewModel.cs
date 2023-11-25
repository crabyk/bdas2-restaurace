using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
	internal class MenuViewModel : BindableBase
	{
		public MenuViewModel()
		{
			NavCommand = new MyICommand<string>(OnNav);
		}

		private FoodViewModel foodViewModel = new FoodViewModel();
		private DrinkViewModel drinkViewModel = new DrinkViewModel();

		private BindableBase _CurrentViewModel;

		public BindableBase CurrentViewModel
		{
			get { return _CurrentViewModel; }
			set { SetProperty(ref _CurrentViewModel, value); }
		}

		public MyICommand<string> NavCommand { get; private set; }

		private void OnNav(string destination)
		{

			switch (destination)
			{
				case "food":
					CurrentViewModel = foodViewModel;
					break;
				case "drinks":
					CurrentViewModel = drinkViewModel;
					break;
				default:
					CurrentViewModel = null;
					break;
			}
		}
	}
}
