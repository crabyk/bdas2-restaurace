using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class OrderViewModel : ViewModelBase<Order, OrderController>
    {
        private ObservableCollection<PaymentType> paymentTypes;

        private ObservableCollection<Address> addresses;

        private ObservableCollection<Item> orderItems;
        private Item selectedOrderItem;

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
            PaymentTypes = new ObservableCollection<PaymentType>(new PaymentTypeController().GetAll());
            Addresses = new ObservableCollection<Address>(new AddressController().GetAll());

            List<Food> foods = new FoodController().GetAll();
            List<Drink> drinks = new DrinkController().GetAll();
            OrderItems = new ObservableCollection<Item>();

            foreach (Food food in foods)
                OrderItems.Add(food);
            foreach (Drink drink in drinks)
                OrderItems.Add(drink);

            RemoveOrderItem = new RelayCommand(RemoveOrderItemMethod, CanRemoveOrderItemMethod);
            AddOrderItem = new RelayCommand(AddOrderItemMethod, CanAddOrderItemMethod);
        }

        private bool CanAddOrderItemMethod(object obj)
        {
            return true;
        }

        private void AddOrderItemMethod(object obj)
        {
            // new OrderItemController().Add(SelectedOrderItem, SelectedItem);
            SelectedItem.Items.Add(SelectedOrderItem);
        }

        private bool CanRemoveOrderItemMethod(object obj)
        {
            return true;
        }

        private void RemoveOrderItemMethod(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
