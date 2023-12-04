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
	public class ViewModelBase<T, U> : BindableBase 
        where T : ModelBase, new()
        where U : Controller<T>, new()
    {
        // private Controller<object> controller;
        protected U controller;
        protected T selectedItem;
        protected ObservableCollection<T> items;
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

        public ViewModelBase(U controller)
		{
            SelectedItem = new T();
            Items = new ObservableCollection<T>();
            this.controller = controller;
            ClearSelected = new RelayCommand(ClearMethod, CanClearMethod);
            Create = new RelayCommand(CreateMethod, CanCreateMethod);
            Delete = new RelayCommand(DeleteMethod, CanDeleteMethod);
            Update = new RelayCommand(UpdateMethod, CanUpdateMethod);
            Load();
		}

        protected bool CanUpdateMethod(object obj)
        {
            return SelectedItem != null;
        }

        protected void UpdateMethod(object obj)
        {
            controller.Update(SelectedItem);
            Load();
        }

        protected bool CanDeleteMethod(object obj)
        {
            return SelectedItem != null;
        }

        protected void DeleteMethod(object obj)
        {
            controller.Delete(SelectedItem.ID.ToString());
            Load();
        }

        protected bool CanCreateMethod(object obj)
        {
            return true;
        }

        protected void CreateMethod(object obj)
        {
            controller.Add(SelectedItem);
            Load();
        }

        protected bool CanClearMethod(object obj)
        {
            return true;
        }

        protected void ClearMethod(object obj)
        {
            SelectedItem = new T();
        }


        protected async void Load()
		{
            List<T> result = await Task.Run(() => controller.GetAll());
            ObservableCollection<T> items = new ObservableCollection<T>(result);
            Items = items;
		}
	}
}
