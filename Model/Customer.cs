using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	internal class Customer : Person
	{
		public Food? Order { get; set; }	
		public Customer(
			int id,
			string firstName,
			string lastName,
			DateTime birthDate,
			string phoneNumber,
			string email,
			Address address

			) : base(id, firstName, lastName, birthDate, phoneNumber, email, address)
		{
		}

	}
}
