using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class ReservationViewModel : RouteNavigation
    {
        private ObservableCollection<Table> tableList = new ObservableCollection<Table>();
        private Table selectedTable;

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
        public ICommand CreateReservation { get; set; }

        public ReservationViewModel()
        {
            Customer = new Customer();
            Address = new Address();
            CreateReservation = new RelayCommand(Create, CanCreate);
            Load();
        }

        public bool CanCreate(object obj)
        {
            return true;
        }

        public void Create(object obj)
        {

            Address newAddress = new AddressController().Add(Address);
            Customer.Address = newAddress;
            Customer newCustomer = new CustomerController().Add(Customer);

            Reservation newReservation = new Reservation()
            {
                ReservationDate = ReservationDate,
                NumberOfPeople = NumberOfPeople,
                Customer = newCustomer,
                Table = SelectedTable
            };

            new ReservationController().Add(newReservation);
        }

        public void Load()
        {
            LoadTables();
        }

        private async void LoadTables()
        {
            List<Table> tables = await Task.Run(() => new TableController().GetAll());
            TableList = new ObservableCollection<Table>(tables);
        }
    }
}
