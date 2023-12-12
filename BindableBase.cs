using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace
{
	public class BindableBase : INotifyPropertyChanged
	{
        public event EventHandler WindowClose;
        protected virtual void OnWindowClose()
        {
            WindowClose?.Invoke(this, EventArgs.Empty);
        }

		public delegate void WindowChangeEventHandler(BindableBase newWindow);

        public event WindowChangeEventHandler WindowChange;
        protected virtual void OnWindowChange(BindableBase newWindow)
        {
			WindowChange?.Invoke(newWindow);
        }

        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(member, val)) return;

			member = val;
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}
}
