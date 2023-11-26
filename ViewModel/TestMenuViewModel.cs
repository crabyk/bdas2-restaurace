using BDAS2_Restaurace.Model;
using System.Collections.ObjectModel;

namespace BDAS2_Restaurace.ViewModel
{
    public class TestMenuViewModel : BindableBase
    {
        public ObservableCollection<Food> Food { get; set; }

        public TestMenuViewModel()
        {
            Food = new ObservableCollection<Food>()
            {
                new Food { ID = 1, Name = "Gulaš se šesti", Price = 120, Weight = 150, Recipe = "Some recipe" },
                new Food { ID = 2, Name = "Svičkova na smetaně", Price = 150, Weight = 160, Recipe = "Some recipe" },
                new Food { ID = 3, Name = "Řízek s bramborem", Price = 130, Weight = 135, Recipe = "Some recipe" },
                new Food { ID = 4, Name = "Koprovka s vejcem", Price = 100, Weight = 140, Recipe = "Some recipe" },
                new Food { ID = 5, Name = "Vyborny jidlo", Price = 120, Weight = 110, Recipe = "Magic"},
                new Food { ID = 6, Name = "Jeste lepsi jidlo", Price = 140, Weight = 90, Recipe = "Magic times 2"}
            };
        }
    }
}
