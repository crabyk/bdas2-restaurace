using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	internal class Employee : Person
	{
		public Address Address { get; set; }

		public Employee(
			int id,
			string firstName,
			string lastName,
			DateTime birthDate,
			string phoneNumber,
			string email,
			Address address

			) : base(id, firstName, lastName, birthDate, phoneNumber, email, address)
		{
			Address = address;
		}
	}
}
