using BDAS2_Restaurace.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.models
{
	internal class Customer : Person
	{
		public Food? Order { get; set; }	
		public Customer(
			string firstName,
			string lastName,
			DateTime birthDate,
			string phoneNumber,
			string email

			) : base(firstName, lastName, birthDate, phoneNumber, email)
		{
		}

	}
}
