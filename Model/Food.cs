using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class Food : Item
    {
        private double weight;
        private string recipe;

        [Range(1, 1000, ErrorMessage = "Hodnota musí být od 10g do 2500g")]
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
