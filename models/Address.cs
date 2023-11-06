using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.models
{
	internal class Address
	{
		public string StreetName { get; set; }
		public string CityName { get; set; }
		public string UnitNumber { get; set; }	
		public string PostalCode { get; set; }
		public string Country { get; set; }

		public Address(string streetName, string cityName, string unitNumber, string postalCode, string country)
		{
			StreetName = streetName;
			CityName = cityName;
			UnitNumber = unitNumber;
			PostalCode = postalCode;
			Country = country;
		}

		public override string ToString()
		{
			string result = "";
			result += $"Ulice: {StreetName}";
			result += $"\nObec: {CityName}";
			result += $"\nCislo popisne: {UnitNumber}";
			result += $"\nPSC: {PostalCode}";
			result += $"\nZeme: {Country}";

			return result;
		}
	}
}
