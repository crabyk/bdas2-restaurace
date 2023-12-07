using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class TestMenuViewModel : BindableBase
    {
        public ObservableCollection<Item> Items { get; set; }

        private Item selectedMenuItem;

        public Item SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set { SetProperty(ref selectedMenuItem, value); }
        }

        private bool CanOrderItem(object param)
        {
            return true;
        }

        private ICommand orderItemCommand;

        public ICommand OrderItemCommand
        {
            get
            {
                if (orderItemCommand == null)
                {
                    orderItemCommand = new RelayCommand(param => OrderItem((Item)param), CanOrderItem);
                }
                return orderItemCommand;
            }
        }

        private void OrderItem(Item item)
        {
            SelectedMenuItem = item;
            // pak se nahradi za create metodu
        }

        public TestMenuViewModel()
        {
            // potreba to pak predelat na asynchronni verzi
            var food = new FoodController().GetFoodFromView();
            var drinks = new DrinkController().GetDrinksFromView();

            Items = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
        }
    }
}
