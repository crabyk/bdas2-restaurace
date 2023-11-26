using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
	public class OrderViewModel : RouteNavigation
	{
		private Food selectedFood;
		private Drink selectedDrink;

		public Food SelectedFood
		{
			get { return selectedFood; }
			set
			{
				selectedFood = value;
				SelectedItems.Add(value);	
				OnPropertyChanged(nameof(selectedFood));	
			}
		}

		public Drink SelectedDrink
		{
			get { return selectedDrink; }
			set
			{
				selectedDrink = value;
				SelectedItems.Add(value);
				OnPropertyChanged(nameof(selectedDrink));
			}
		}
		public Customer Customer { get; set; }
		public Address Address { get; set; }

		public ObservableCollection<Drink> Drinks
		{
			get;
			set;
		}

		public ObservableCollection<Food> Food
		{
			get;
			set;
		}

		public ObservableCollection<Item> SelectedItems
		{
			get;
			set;
		}

		public OrderViewModel()
		{
			Load();
			SelectedItems = new ObservableCollection<Item>();
			// Routes.Add(new Route("menu", new MenuViewModel()));
		}


		public void Load()
		{
			Food = new ObservableCollection<Food>(FoodController.GetAll());
			Drinks = new ObservableCollection<Drink>(DrinkController.GetAll());
		}
	}
}
