using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
	public class DrinkViewModel : BindableBase
	{

        private Drink selectedDrink;
        private ObservableCollection<Drink> drinks;
        public ObservableCollection<Drink> Drinks
		{
			get { return drinks; }
			set
            {
                drinks = value;
                OnPropertyChanged(nameof(Drinks));
            }
		}
		public Drink SelectedDrink
		{
			get { return selectedDrink; }
			set
			{
				selectedDrink = value == null ? new Drink() : (Drink)value.Clone();
				OnPropertyChanged(nameof(SelectedDrink));	
			}
		}

        public ICommand ClearSelected { get; set; }
        public ICommand Create { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Update { get; set; }

        public DrinkViewModel()
		{
            ClearSelected = new RelayCommand(ClearMethod, CanClearMethod);
            Create = new RelayCommand(CreateMethod, CanCreateMethod);
            Delete = new RelayCommand(DeleteMethod, CanDeleteMethod);
            Update = new RelayCommand(UpdateMethod, CanUpdateMethod);
            Load();
		}

        private bool CanUpdateMethod(object obj)
        {
            return true;
        }

        private void UpdateMethod(object obj)
        {
            new DrinkController().Update(SelectedDrink);
            Load();
        }

        private bool CanDeleteMethod(object obj)
        {
            return SelectedDrink != null;
        }

        private void DeleteMethod(object obj)
        {
            new DrinkController().Delete(SelectedDrink.ID.ToString());
            Load();
        }

        private bool CanCreateMethod(object obj)
        {
            return true;
        }

        private void CreateMethod(object obj)
        {
            new DrinkController().Add(SelectedDrink);
            Load();
        }

        public bool CanClearMethod(object obj)
        {
            return true;
        }

        public void ClearMethod(object obj)
        {
            SelectedDrink = new Drink();
        }



        /*
		public void OnDrinkSelected(Drink drink)
		{
			DrinkSelected?.Invoke(this, drink);
		}
		*/

        public void Load()
        {
            ObservableCollection<Drink> data = new ObservableCollection<Drink>(new DrinkController().GetAll());

            /*
			data.Add(new Drink { ID = 1, Name = "Točená kofola", Price = 20, Volume = 300 });
			data.Add(new Drink { ID = 2, Name = "Točená kofola", Price = 35, Volume = 500 });
			data.Add(new Drink { ID = 3, Name = "Radegast 12", Price = 130, Volume = 500 });
			*/

            Drinks = data;
        }
    }
}
