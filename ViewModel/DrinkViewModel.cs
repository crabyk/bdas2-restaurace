using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
	public class DrinkViewModel : BindableBase
	{
		private Drink selectedDrink;
		public ObservableCollection<Drink> Drinks
		{
			get;
			set;
		}
		public Drink SelectedDrink
		{
			get { return selectedDrink; }
			set
			{
				selectedDrink = value;
				OnPropertyChanged(nameof(SelectedDrink));	
			}
		}
		public DrinkViewModel()
		{
			Load();
		}


		/*
		public void OnDrinkSelected(Drink drink)
		{
			DrinkSelected?.Invoke(this, drink);
		}
		*/

		public void Load()
		{
			ObservableCollection<Drink> data = new ObservableCollection<Drink>(DrinkController.GetAll());

			/*
			data.Add(new Drink { ID = 1, Name = "Točená kofola", Price = 20, Volume = 300 });
			data.Add(new Drink { ID = 2, Name = "Točená kofola", Price = 35, Volume = 500 });
			data.Add(new Drink { ID = 3, Name = "Radegast 12", Price = 130, Volume = 500 });
			*/

			Drinks = data;
		}
	}
}
