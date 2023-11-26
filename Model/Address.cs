using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	public class Address
	{
		private string streetName;
		private string cityName;
		private string unitNumber;
		private string postalCode;
		private string country;

		public int ID { get; set; }
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

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
