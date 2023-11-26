using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	public class Drink : Item
	{
		private double volume;

		public double Volume
		{
			get { return volume; }

			set
			{
				volume = value;
				RaisePropertyChanged(nameof(Volume));
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
