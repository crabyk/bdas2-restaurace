using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	public class Food : Item
	{
		private double weight;
		private string recipe;

		public double Weight
		{
			get { return weight; }

			set
			{
				weight = value;
				RaisePropertyChanged("Weight");
			}
		}

		public string Recipe
		{
			get { return recipe; }

			set
			{
				recipe = value;
				RaisePropertyChanged("Recipe");
			}
		}
	}
}
