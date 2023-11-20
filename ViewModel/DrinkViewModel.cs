using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
	public class DrinkViewModel : BindableBase
	{
		public DrinkViewModel()
		{
			Load();
		}

		public ObservableCollection<Drink> Drink
		{
			get;
			set;
		}

		public void Load()
		{
			ObservableCollection<Drink> data = new ObservableCollection<Drink>(DrinkController.GetAll());

			/*
			data.Add(new Drink { ID = 1, Name = "Točená kofola", Price = 20, Volume = 300 });
			data.Add(new Drink { ID = 2, Name = "Točená kofola", Price = 35, Volume = 500 });
			data.Add(new Drink { ID = 3, Name = "Radegast 12", Price = 130, Volume = 500 });
			*/

			Drink = data;
		}
	}
}
