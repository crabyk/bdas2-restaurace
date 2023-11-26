﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace
{
	public class RelayCommand : ICommand
	{
		public event EventHandler? CanExecuteChanged;

		public Action<object> _Execute { get; set; } 

		public Predicate<object> _CanExecute { get; set; }

		public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
		{
			_Execute = executeMethod;
			_CanExecute = canExecuteMethod;
		}

		public bool CanExecute(object? parameter)
		{
			return _CanExecute(parameter);
		}

		public void Execute(object? parameter)
		{
			_Execute(parameter);
		}
	}
}
