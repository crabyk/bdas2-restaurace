using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.ObjectModel;

namespace BDAS2_Restaurace.ViewModel
{
	public class FoodViewModel : ViewModelBase<Food, FoodController>
	{
		public FoodViewModel() : base (new FoodController())
		{
		}
    }
}
