using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class Address : ModelBase
    {
        private string streetName;
        private string cityName;
        private string unitNumber;
        private string postalCode;
        private string country;

        public string StreetName
        {
            get { return streetName; }
            set
            {
                streetName = value;
                RaisePropertyChanged(nameof(StreetName));
            }
        }

        public string CityName
        {
            get { return cityName; }
            set
            {
                cityName = value;
                RaisePropertyChanged(nameof(CityName));
            }
        }

        public string UnitNumber
        {
            get { return unitNumber; }
            set
            {
                unitNumber = value;
                RaisePropertyChanged(nameof(UnitNumber));
            }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                postalCode = value;
                RaisePropertyChanged(nameof(PostalCode));
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                RaisePropertyChanged(nameof(Country));
            }
        }
    }
}
