using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class Food : Item
    {
        private double weight;
        private string recipe;

        [Required(ErrorMessage = "Hmotnost je povinná")]
        public double Weight
        {
            get { return weight; }

            set
            {
                weight = value;
                 OnPropertyChanged("Weight");
            }
        }

        [Required(ErrorMessage = "Recept je povinný")]
        public string Recipe
        {
            get { return recipe; }

            set
            {
                recipe = value;
                 OnPropertyChanged("Recipe");
            }
        }
    }
}
