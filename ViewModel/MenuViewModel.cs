using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace BDAS2_Restaurace.ViewModel
{
    public class MenuViewModel : BindableBase
    {
        public ObservableCollection<Item> Items { get; set; }

        public MenuViewModel()
        {
            // potreba to pak predelat na asynchronni verzi
            var food = new FoodController().GetAll();
            var drinks = new DrinkController().GetAll();

            Items = new ObservableCollection<Item>(drinks.Cast<Item>().Concat(food.Cast<Item>()));
        }
    }
}
