using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Errors;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class ViewModelBase<T, U> : RouteNavigation
        where T : ModelBase, new()
        where U : Controller<T>, new()
    {
        protected U controller;
        protected T? selectedItem;
        protected ObservableCollection<T> items;
        protected ObservableCollection<T> filteredItems;
        private string filterText;

        public ObservableCollection<T> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public T? SelectedItem
        {
            get { return selectedItem; }
            set
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    selectedItem = (T?)value?.Clone() ?? new T();
                    OnPropertyChanged(nameof(selectedItem));
                });
            }
        }

        public ObservableCollection<T> FilteredItems
        {
            get { return filteredItems; }
            set
            {
                filteredItems = value;
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ApplyFilter();
                });
                OnPropertyChanged(nameof(FilterText));
            }
        }

        public ICommand ClearSelected { get; set; }
        public ICommand Create { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Update { get; set; }
        public ICommand ClearFilter { get; set; }


        public ViewModelBase(U controller)
        {
            SelectedItem = new T();
            Items = new ObservableCollection<T>();

            this.controller = controller;
            ClearSelected = new RelayCommand(ClearMethod, CanClearMethod);
            Create = new RelayCommand(CreateMethod, CanCreateMethod);
            Delete = new RelayCommand(DeleteMethod, CanDeleteMethod);
            Update = new RelayCommand(UpdateMethod, CanUpdateMethod);
            ClearFilter = new RelayCommand(ClearFilterMethod, CanClearFilterMethod);

            Load();
        }

        protected virtual bool CanUpdateMethod(object obj)
        {
            return PropertyValidateModel.Validate(SelectedItem) && Items.Any(i => i.ID == SelectedItem.ID);
        }

        protected virtual void UpdateMethod(object obj)
        {
            try
            {
                controller.Update(SelectedItem);
            }
            catch
            {
                ErrorHandler.OpenDialog(ErrorType.Update);
            }
            Load();
        }

        protected virtual bool CanDeleteMethod(object obj)
        {
            return PropertyValidateModel.Validate(SelectedItem) && Items.Any(i => i.ID == SelectedItem.ID);
        }

        protected virtual void DeleteMethod(object obj)
        {
            try
            {
                controller.Delete(SelectedItem.ID.ToString());
            }
            catch
            {
                ErrorHandler.OpenDialog(ErrorType.Delete);
            }
            Load();
        }

        protected virtual bool CanCreateMethod(object obj)
        {
            /*
            ICollection<ValidationResult> results = null;
            return PropertyValidateModel.Validate(SelectedItem, out results);
            */
            return PropertyValidateModel.Validate(SelectedItem);
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

        protected virtual bool CanClearFilterMethod(object obj)
        {
            return true;
        }

        protected virtual void ClearFilterMethod(object obj)
        {
            FilterText = string.Empty;
        }

        protected virtual async void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                FilteredItems = Items;
            }
            else
            {
                var filteredList = await Task.Run(() =>
                {
                    return new ObservableCollection<T>(
                        Items.Where(item => IsMatchingFilter(item))
                    );
                });

                FilteredItems = filteredList;
            }
        }

        protected virtual bool IsMatchingFilter(T item)
        {
            return true;
        }

        protected virtual async void Load()
        {
            try
            {
                List<T> result = await Task.Run(() => controller.GetAll());
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ObservableCollection<T> items = new ObservableCollection<T>(result);
                    Items = items;
                    FilteredItems = items;
                });
            }
            catch
            {
                ErrorHandler.OpenDialog("Chyba při načítání dat", "Neočekávaná chyba při získávání dat z databáze");
            }
        }
    }
}
