using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
	public class TableViewModel : BindableBase
	{
		private Type? type;
		private object? data;
		public object? Data
		{
			get { return data; }
			set
			{
				data = value;
				OnPropertyChanged(nameof(Data));
			}
		}
		public TableViewModel(Type tableType)
		{

			data = Activator.CreateInstance(tableType);
			Load();
		}

		public void Load()
		{
			// Data.FirstName = "Martin";
			// Data.LastName = "Seidl";
		}
	}
}
