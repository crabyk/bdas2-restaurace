using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;

namespace BDAS2_Restaurace.ViewModel
{
    public class MenuViewModel : RouteNavigation
    {
        private DrinkViewModel drinkViewModel;

        public DrinkViewModel DrinkViewModel => drinkViewModel;

        private FoodViewModel foodViewModel;

        public FoodViewModel FoodViewModel => foodViewModel;

        private Drink selectedDrink;
        public Drink SelectedDrink
        {
            get { return DrinkViewModel.SelectedItem; }
            set
            {
                selectedDrink = value;
                OnPropertyChanged(nameof(SelectedDrink));
            }

        }
        public MenuViewModel() : base()
        {

            this.Routes.Add(new Route<DrinkViewModel>("drinks"));
            this.Routes.Add(new Route<FoodViewModel>("food"));
            this.Routes.Add(new Route<TestMenuViewModel>("testmenu"));

        }
        /*
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
		*/
    }
}
