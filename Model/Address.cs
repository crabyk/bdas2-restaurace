using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BDAS2_Restaurace.Model
{
    public class Address : ModelBase
    {
        private string streetName;
        private string cityName;
        private string unitNumber;
        private string postalCode;
        private string country;

        public Address()
        {
            streetName = string.Empty;
            cityName = string.Empty;
            unitNumber = string.Empty;
            postalCode = string.Empty;
            country = string.Empty;
        }

        [Required(ErrorMessage = "Ulice je povinná")]
        public string StreetName
        {
            get { return streetName; }
            set
            {
                streetName = value;
                OnPropertyChanged(nameof(StreetName));
            }
        }

        [Required(ErrorMessage = "Obec je povinná")]
        public string CityName
        {
            get { return cityName; }
            set
            {
                cityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }

        [Required(ErrorMessage = "Číslo popisné je povinné")]
        public string UnitNumber
        {
            get { return unitNumber; }
            set
            {
                unitNumber = value;
                OnPropertyChanged(nameof(UnitNumber));
            }
        }

        [Required(ErrorMessage = "PSČ je povinné")]
        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        [Required(ErrorMessage = "Země je pivnná")]
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
