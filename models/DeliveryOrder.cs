﻿using BDAS2_Restaurace.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2SemestralkaDemo.models
{
	internal class DeliveryOrder : Order
	{
		Address Address { get; }
	
		public DeliveryOrder (DateTime orderDate, Customer customer, Address address) : base (orderDate, customer)
		{
			Address = address;
		}

		public override string ToString()
		{
			string result = "";
			result += "Objednavka s sebou\n";
			result += "==================================\n";
			result += Address.ToString();
			result += base.ToString();
			return result;
		}
	}
}