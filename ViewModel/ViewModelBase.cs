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
	public class ViewModelBase<T, U> : RouteNavigation
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

        protected virtual bool CanUpdateMethod(object obj)
        {
            return SelectedItem != null;
        }

        protected virtual void UpdateMethod(object obj)
        {
            controller.Update(SelectedItem);
            Load();
        }

        protected virtual bool CanDeleteMethod(object obj)
        {
            return SelectedItem != null;
        }

        protected virtual void DeleteMethod(object obj)
        {
            controller.Delete(SelectedItem.ID.ToString());
            Load();
        }

        protected virtual bool CanCreateMethod(object obj)
        {
            return true;
        }

        protected virtual void CreateMethod(object obj)
        {
            controller.Add(SelectedItem);
            Load();
        }

        protected virtual bool CanClearMethod(object obj)
        {
            return true;
        }

        protected virtual void ClearMethod(object obj)
        {
            SelectedItem = new T();
        }


        protected virtual async void Load()
		{
            /*
             * 
             * Odkomentovat pro lepsi otestovani async chovani
             * 
             * Jinak GUI okna nezamrzaji coz je fajn
             * 
             * Zaroven se diky tomu da otestovat nove vytvareni instanci ViewModel jen kdyz jsou potreba, 
             * protoze metoda Load() se spousti jen v konstruktoru
             * 
             */
            // await Task.Delay(2000);
            List<T> result = await Task.Run(() => controller.GetAll());
            ObservableCollection<T> items = new ObservableCollection<T>(result);
            Items = items;
		}
	}
}
