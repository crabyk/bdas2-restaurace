using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class OrderViewModel : RouteNavigation
    {
        private double price;
        private Food selectedFood;
        private Drink selectedDrink;
        private PaymentType selectedPaymentType;

        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public Food SelectedFood
        {
            get { return selectedFood; }
            set
            {
                selectedFood = value;
                SelectedItems.Add(value);
                OnPropertyChanged(nameof(SelectedFood));
            }
        }

        public Drink SelectedDrink
        {
            get { return selectedDrink; }
            set
            {
                selectedDrink = value;
                SelectedItems.Add(value);
                OnPropertyChanged(nameof(SelectedDrink));
            }
        }

        public PaymentType SelectedPaymentType
        {
            get { return selectedPaymentType; }
            set
            {
                selectedPaymentType = value;
                OnPropertyChanged(nameof(SelectedPaymentType));
            }
        }

        public Customer Customer { get; set; }
        public Address Address { get; set; }

        public ObservableCollection<Drink> Drinks
        {
            get;
            set;
        }

        public ObservableCollection<Food> Food
        {
            get;
            set;
        }

        public ObservableCollection<PaymentType> PaymentTypes
        {
            get;
            set;
        }

        public ObservableCollection<Item> SelectedItems
        {
            get;
            set;
        }

        public ICommand CreateOrder { get; set; }

        public OrderViewModel()
        {
            Customer = new Customer();
            Address = new Address();
            CreateOrder = new RelayCommand(Create, CanCreate);
            SelectedItems = new ObservableCollection<Item>();
            Load();
            // Routes.Add(new Route("menu", new MenuViewModel()));
        }

        public bool CanCreate(object obj)
        {
            return true;
        }

        public void Create(object obj)
        {

            Address newAddress = AddressController.Add(Address);
            Customer.Address = newAddress;
            Customer newCustomer = CustomerController.Add(Customer);
            Payment newPayment = PaymentController.Add(new Payment()
            {
                Date = DateTime.Now,
                Amount = 100,
                Type = SelectedPaymentType
            });

            //Table newTable = new Table()
            //{
            //    ID = 1,
            //    Number = 10
            //};

            List<Item> items = new List<Item>(SelectedItems);

            Order newOrder = new Order()
            {
                OrderDate = DateTime.Now,
                Customer = newCustomer,
                Address = newAddress,
                Payment = newPayment,
                //Table = newTable,
                Items = items
            };

            OrderController.Add(newOrder);
        }


        public void Load()
        {
            Food = new ObservableCollection<Food>(FoodController.GetAll());
            Drinks = new ObservableCollection<Drink>(DrinkController.GetAll());
            PaymentTypes = new ObservableCollection<PaymentType>(PaymentController.GetAllTypes());
            SelectedPaymentType = PaymentTypes[0];
        }
    }
}
