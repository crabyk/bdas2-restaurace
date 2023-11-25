using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Router
{
	public class Route
	{
		public string Name { get; set; }
		public BindableBase ViewModel { get; set; }

		public Route(string name, BindableBase viewModel)
		{
			Name = name;
			ViewModel = viewModel;
		}
	}
}
