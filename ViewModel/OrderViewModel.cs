using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Errors;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class OrderViewModel : ViewModelBase<Order, OrderController>
    {
        private ObservableCollection<PaymentType> paymentTypes;
        private ObservableCollection<Address> addresses;
        private ObservableCollection<Item> orderItems = new ObservableCollection<Item>();
        private Item selectedOrderItem;
        private Item newOrderItem;

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

        public ObservableCollection<PaymentType> PaymentTypes
        {
            get { return paymentTypes; }
            set
            {
                paymentTypes = value;
                OnPropertyChanged(nameof(PaymentTypes));
            }
        }

        public ObservableCollection<Address> Addresses
        {
            get { return addresses; }
            set
            {
                addresses = value;
                OnPropertyChanged(nameof(Addresses));
            }
        }

        public Item SelectedOrderItem
        {
            get { return selectedOrderItem; }
            set
            {
                selectedOrderItem = value;
                OnPropertyChanged(nameof(SelectedOrderItem));
            }
        }

        public Item NewOrderItem
        {
            get { return newOrderItem; }
            set
            {
                newOrderItem = value;
                OnPropertyChanged(nameof(NewOrderItem));
            }
        }

        public ObservableCollection<Item> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
                OnPropertyChanged(nameof(OrderItems));
            }
        }

        public ICommand RemoveOrderItem { get; set; }
        public ICommand AddOrderItem { get; set; }
        public ICommand SelectOrderItem;
        public ICommand LoadCustomer { get; set; }

        public ICommand OrderItemCommand
        {
            get
            {
                if (SelectOrderItem == null)
                {
                    SelectOrderItem = new RelayCommand(param => OrderItem((Item)param), CanOrderItem);
                }
                return SelectOrderItem;
            }
        }

        private void OrderItem(Item item)
        {
            // SelectedOrderItem = item;
            SelectedItem.Payment.Amount = SelectedItem.Payment.Amount + item.Price;
            SelectedItem.AddItem(item);
        }

        private bool CanOrderItem(object param)
        {
            return true;    
        }

        public OrderViewModel() : base(new OrderController())
        {
            LoadCustomer = new RelayCommand(LoadCustomerMethod, CanLoadCustomerMethod);
            RemoveOrderItem = new RelayCommand(RemoveOrderItemMethod, CanRemoveOrderItemMethod);
            AddOrderItem = new RelayCommand(AddOrderItemMethod, CanAddOrderItemMethod);
            var food = new FoodController().GetFoodMenu();
            var drinks = new DrinkController().GetDrinkMenu();

            OrderItems = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
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

        private async void LoadAddresses()
        {
            List<Address> paymentTypes = await Task.Run(() => new AddressController().GetAll());
            Addresses = new ObservableCollection<Address>(paymentTypes);
        }

        private async void LoadPayments()
        {
            List<PaymentType> paymentTypes = await Task.Run(() => new PaymentTypeController().GetAll());
            PaymentTypes = new ObservableCollection<PaymentType>(paymentTypes);
            SelectedItem.Payment.Type = PaymentTypes.First();
        }

        private async void LoadFood()
        {
            List<Food> foods = await Task.Run(() => new FoodController().GetAll());
            foods.ForEach(f => OrderItems.Add(f));
        }

        private async void LoadItems()
        {
            var food = await Task.Run(() => new FoodController().GetAll());
            var drinks = await Task.Run(() => new DrinkController().GetAll());

            OrderItems = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
        }

        private async void LoadDrinks()
        {
            List<Drink> drinks = await Task.Run(() => new DrinkController().GetAll());
            drinks.ForEach(d => OrderItems.Add(d));
        }


        protected override void Load()
        {
            // LoadAddresses();
            LoadPayments();
            // LoadFood();
            // LoadDrinks();
            // LoadItems();
            base.Load();
        }


        private bool CanAddOrderItemMethod(object obj)
        {
            return SelectedItem.Items.Where(i => i.ID == NewOrderItem.ID).Count() == 0;
        }

        private void AddOrderItemMethod(object obj)
        {
            SelectedItem.Payment.Amount = SelectedItem.Payment.Amount + NewOrderItem.Price;
            SelectedItem.AddItem(NewOrderItem);
        }

        private bool CanRemoveOrderItemMethod(object obj)
        {
            return true;
        }

        private void RemoveOrderItemMethod(object obj)
        {
            if (SelectedOrderItem == null)
                return;

            SelectedItem.Payment.Amount = SelectedItem.Payment.Amount - SelectedOrderItem.Price;
            SelectedItem.RemoveItem(SelectedOrderItem);
            // Load();
        }

        protected override bool CanCreateMethod(object obj)
        {
            return PropertyValidateModel.Validate(SelectedItem.Customer) &&
                PropertyValidateModel.Validate(SelectedItem.Payment.Type);
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
            catch (Exception ex) { }

            OnWindowClose();
        }

        protected override bool IsMatchingFilter(Order item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return
                item.Customer.FirstName.ToLower().Contains(filterTextLower) ||
                item.Customer.LastName.ToLower().Contains(filterTextLower) ||
                item.Customer.Email.ToLower().Contains(filterTextLower) ||
                (item.Customer.User != null && item.Customer.User.Login.ToLower().Contains(filterTextLower));
        }
    }
}
