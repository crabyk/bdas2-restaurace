using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Errors;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class ReservationViewModel : ViewModelBase<Reservation, ReservationController>
    {
        private ObservableCollection<Table> tableList = new ObservableCollection<Table>();
        private Table selectedTable;

        private string username;
        private string password;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ObservableCollection<Table> TableList
        {
            get { return tableList; }
            set
            {
                tableList = value;
                OnPropertyChanged(nameof(TableList));
            }
        }

        public Table SelectedTable
        {
            get { return selectedTable; }
            set
            {
                selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        public int NumberOfPeople { get; set; }
        public DateTime ReservationDate { get; set; }
        public Customer Customer { get; set; }
        public Address Address { get; set; }

        public ICommand LoadCustomer { get; set; }

        public ReservationViewModel() : base(new ReservationController())
        {
            Customer = new Customer();
            Address = new Address();
            LoadCustomer = new RelayCommand(LoadCustomerMethod, CanLoadCustomerMethod);
        }

        private bool CanLoadCustomerMethod(object obj)
        {
            return true;
        }

        private void LoadCustomerMethod(object obj)
        {
            try
            {
                User? user = new UserController().Login(Username, Password);
                if (user == null)
                    return;

                if (user.Role.ID != 2)
                    return;

                Customer loadedCustomer = new CustomerController().GetByUser(user);

                SelectedItem.Customer = loadedCustomer ?? new Customer();
                Username = string.Empty;
                Password = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorHandler.OpenDialog("Nepodařilo se načíst zákaznické údaje", "Chyba při přihlášní");
            }


        }

        protected override bool CanCreateMethod(object obj)
        {
            return base.CanCreateMethod(obj) &&
                PropertyValidateModel.Validate(SelectedItem.Customer) &&
                PropertyValidateModel.Validate(SelectedItem.Customer.Address);
        }

        protected override void CreateMethod(object obj)
        {
            try
            {
                base.CreateMethod(obj);

                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.None;

                MessageBox.Show("Objednávka vytvořena", "Objednávka", button, icon, MessageBoxResult.OK);
            }
            catch (Exception ex)
            {
                ErrorHandler.OpenDialog("Objednávka vytvořena", "Objednávka");
            }

            OnWindowClose();
        }

        protected override void Load()
        {
            LoadTables();
            base.Load();
        }

        private async void LoadTables()
        {
            List<Table> tables = await Task.Run(() => new TableController().GetAll());
            TableList = new ObservableCollection<Table>(tables);
        }
    }
}
