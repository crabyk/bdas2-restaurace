namespace BDAS2_Restaurace.Model
{
    public class Role : ModelBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}
