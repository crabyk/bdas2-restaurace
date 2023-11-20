﻿using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
	public class FoodViewModel : BindableBase
	{
		public FoodViewModel()
		{
			Load();
		}

		public ObservableCollection<Food> Food
		{
			get;
			set;
		}

		public void Load()
		{
			ObservableCollection<Food> data = new ObservableCollection<Food>(FoodController.GetAll());

			/*
			data.Add(new Food { ID = 1, Name = "Gulaš se šesti", Price = 120, Weight = 150, Recipe = "Some recipe" });
			data.Add(new Food { ID = 2, Name = "Svičkova na smetaně", Price = 150, Weight = 160, Recipe = "Some recipe" });
			data.Add(new Food { ID = 3, Name = "Řízek s bramborem", Price = 130, Weight = 135, Recipe = "Some recipe" });
			data.Add(new Food { ID = 4, Name = "Koprovka s vejcem", Price = 100, Weight = 140, Recipe = "Some recipe" });
			*/


			Food = data;
		}
	}
}