using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.models
{
	internal class Employee : Person
	{
		public Address Address { get; set; }

		public Employee(
			string firstName,
			string lastName,
			DateTime birthDate,
			string phoneNumber,
			string email,
			Address address

			) : base(firstName, lastName, birthDate, phoneNumber, email)
		{
			Address = address;
		}
	}
}
