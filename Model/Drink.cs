namespace BDAS2_Restaurace.Model
{
    public class Drink : Item
    {
        private double volume;

        public double Volume
        {
            get { return volume; }

            set
            {
                volume = value;
                 OnPropertyChanged(nameof(Volume));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
