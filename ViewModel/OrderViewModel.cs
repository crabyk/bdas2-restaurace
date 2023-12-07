using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BDAS2_Restaurace.ViewModel
{
    public class OrderViewModel : ViewModelBase<Order, OrderController>
    {
        private ObservableCollection<PaymentType> paymentTypes;

        private ObservableCollection<Address> addresses;

        private ObservableCollection<Item> orderItems = new ObservableCollection<Item>();
        private Item selectedOrderItem;
        private Item newOrderItem;

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

        public OrderViewModel() : base(new OrderController())
        {
            // PaymentTypes = new ObservableCollection<PaymentType>(new PaymentTypeController().GetAll());
            // Addresses = new ObservableCollection<Address>(new AddressController().GetAll());
            RemoveOrderItem = new RelayCommand(RemoveOrderItemMethod, CanRemoveOrderItemMethod);
            AddOrderItem = new RelayCommand(AddOrderItemMethod, CanAddOrderItemMethod);
        }

        protected override void CreateMethod(object obj)
        {
            base.CreateMethod(obj);
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
        }

        private async void LoadFood()
        {
            List<Food> foods = await Task.Run(() => new FoodController().GetAll());
            foods.ForEach(f => OrderItems.Add(f));
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
            LoadFood();
            LoadDrinks();
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

    }
}
