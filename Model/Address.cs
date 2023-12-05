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
                 OnPropertyChanged(nameof(StreetName));
            }
        }

        public string CityName
        {
            get { return cityName; }
            set
            {
                cityName = value;
                 OnPropertyChanged(nameof(CityName));
            }
        }

        public string UnitNumber
        {
            get { return unitNumber; }
            set
            {
                unitNumber = value;
                 OnPropertyChanged(nameof(UnitNumber));
            }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                postalCode = value;
                 OnPropertyChanged(nameof(PostalCode));
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                 OnPropertyChanged(nameof(Country));
            }
        }

        public override string ToString()
        {
            return $"{StreetName} {UnitNumber}\n{PostalCode} {CityName}\n{Country}";
        }
    }
}
