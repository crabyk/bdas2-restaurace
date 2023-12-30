using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
    public class MenuViewModel : BindableBase
    {
        private readonly FoodController foodController;
        private readonly DrinkController drinkController;

        private ObservableCollection<Item> items;
        public ObservableCollection<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Item> topItems;
        public ObservableCollection<Item> TopItems
        {
            get { return topItems; }
            set
            {
                topItems = value;
                OnPropertyChanged(nameof(TopItems));
            }
        }

        public MenuViewModel()
        {
            foodController = new FoodController();
            drinkController = new DrinkController();

            //LoadDataAsync();
            var food = foodController.GetFoodMenu();
            var drinks = drinkController.GetDrinkMenu();

            var topFood = foodController.GetMostOrderedFood();
            var topDrink = drinkController.GetMostOrderedDrink();

            List<Item> topItemsList = new List<Item>
            {
                topFood,
                topDrink
            };

            Items = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
            TopItems = new ObservableCollection<Item>(topItemsList);
        }

        private async void LoadDataAsync()
        {
            var foodTask = Task.Run(() => foodController.GetFoodMenu());
            var drinksTask = Task.Run(() => drinkController.GetDrinkMenu());
            var topFoodTask = Task.Run(() => foodController.GetMostOrderedFood());
            var topDrinkTask = Task.Run(() => drinkController.GetMostOrderedDrink());

            await Task.WhenAll(foodTask, drinksTask, topFoodTask, topDrinkTask);

            var food = foodTask.Result;
            var drinks = drinksTask.Result;
            var topFood = topFoodTask.Result;
            var topDrink = topDrinkTask.Result;

            var topItemsList = new List<Item> { topFood, topDrink };

            Items = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
            TopItems = new ObservableCollection<Item>(topItemsList);
        }
    }
}
