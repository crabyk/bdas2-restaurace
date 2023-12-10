using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BDAS2_Restaurace.ViewModel
{
    public class MenuViewModel : BindableBase
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> TopItems { get; set; }

        public MenuViewModel()
        {
            // potreba to pak predelat na asynchronni verzi
            var food = new FoodController().GetAll();
            var drinks = new DrinkController().GetAll();

            var topFood = new FoodController().GetMostOrderedFood();
            var topDrink = new DrinkController().GetMostOrderedDrink();

            List<Item> topItems = new List<Item>
            {
                topFood,
                topDrink
            };

            Items = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
            TopItems = new ObservableCollection<Item>(topItems);
        }
    }
}
