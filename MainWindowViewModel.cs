﻿using BDAS2_Restaurace.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace
{
	class MainWindowViewModel : BindableBase
	{
		public MainWindowViewModel()
		{
			NavCommand = new MyICommand<string>(OnNav);
		}

		private MenuViewModel menuViewModel = new MenuViewModel();

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
				case "menu":
					CurrentViewModel = menuViewModel;
					break;
				default:
					CurrentViewModel = null;
					break;
			}
		}
	}
}
