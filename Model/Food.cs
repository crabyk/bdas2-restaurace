namespace BDAS2_Restaurace.Model
{
    public class Food : Item
    {
        private double weight;
        private string recipe;

        public double Weight
        {
            get { return weight; }

            set
            {
                weight = value;
                 OnPropertyChanged("Weight");
            }
        }

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
