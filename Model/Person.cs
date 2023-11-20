﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
	internal abstract class Person
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }

		public Address Address { get; set; }	

		public Person(
			int id,
			string firstName, 
			string lastName, 
			DateTime birthDate, 
			string phoneNumber, 
			string email,
			Address address
			)
		{
			if (firstName.Length == 0 || lastName.Length == 0)
				throw new ArgumentException("First name or last name is empty", nameof(firstName));

			if (!Regex.Match(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").Success)
				throw new ArgumentException("Špatný formát pro email", nameof(firstName));

			ID = id;
			FirstName = firstName;
			LastName = lastName;
			BirthDate = birthDate;
			PhoneNumber = phoneNumber;
			Email = email;
			Address = address;
		}

		public int Age()
		{
			DateTime currentDate = DateTime.Now;
			int age = currentDate.Year - BirthDate.Year;
			if (BirthDate > currentDate.AddYears(-age))
				age--;

			return age;
		}

		public override string ToString()
		{
			string result = "";
			result += $"Jmeno: {FirstName} {LastName}";
			result += $"\nDatum narozeni: {BirthDate}";
			result += $"\nVek: {Age()}";
			result += $"\nPhone num.: {PhoneNumber}";
			result += $"\nEmail: {Email}";
			return result;
		}
	}
}