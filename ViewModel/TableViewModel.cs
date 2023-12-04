using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
	public class TableViewModel<T, U> : BindableBase 
        where T : ICloneable, new()
        where U : Controller<T>
	{
        // private Controller<object> controller;
        private U controller;
		private T selectedItem;
		private ObservableCollection<T> items;
        public ObservableCollection<T> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = (T)value.Clone();
                OnPropertyChanged(nameof(selectedItem));
            }
        }
        public ICommand ClearSelected { get; set; }
        public ICommand Create { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Update { get; set; }

        public TableViewModel(U controller)
		{
            this.controller = controller;
            ClearSelected = new RelayCommand(ClearMethod, CanClearMethod);
            Create = new RelayCommand(CreateMethod, CanCreateMethod);
            Delete = new RelayCommand(DeleteMethod, CanDeleteMethod);
            Update = new RelayCommand(UpdateMethod, CanUpdateMethod);
            Load();
		}

        private bool CanUpdateMethod(object obj)
        {
            return selectedItem != null;
        }

        private void UpdateMethod(object obj)
        {
            controller.Update(selectedItem);
            Load();
        }

        private bool CanDeleteMethod(object obj)
        {
            return selectedItem != null;
        }

        private void DeleteMethod(object obj)
        {
            // controller.Delete();
            Load();
        }

        private bool CanCreateMethod(object obj)
        {
            return true;
        }

        private void CreateMethod(object obj)
        {
            controller.Add(selectedItem);
            Load();
        }

        public bool CanClearMethod(object obj)
        {
            return true;
        }

        public void ClearMethod(object obj)
        {
            selectedItem = new T();
        }


        public void Load()
		{
            ObservableCollection<T> items = new ObservableCollection<T>(controller.GetAll());
		}
	}
}
