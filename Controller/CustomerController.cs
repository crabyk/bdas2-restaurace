﻿using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace BDAS2_Restaurace.Controller
{
	public class CustomerController : Controller<Customer>
	{

        public override Customer? Add(Customer item)
		{
			Customer? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_zakaznika";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = item.BirthDate;
                    comm.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = item.PhoneNumber;
                    comm.Parameters.Add("p_email", OracleDbType.Varchar2).Value = item.Email;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;
                    comm.Parameters.Add("p_id_zakaznik", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_zakaznik"].Value).Value;
                }

                result = item;
                result.ID = Convert.ToInt32(newId);
            }

            return result;
        }

        public override int Delete(string id)
		{
			int result = 0;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "smazat_zakaznika";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_zakaznik", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Customer? Get(string id)
		{
			Customer? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_zakaznika";
                    comm.CommandType = CommandType.StoredProcedure;
    
                    comm.Parameters.Add("p_id_zakaznik", id);

                    OracleParameter firstName = new OracleParameter("p_jmeno", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(firstName);
                    OracleParameter lastName = new OracleParameter("p_prijmeni", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(lastName);
                    OracleParameter birthDate = new OracleParameter("p_datum_narozeni", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(birthDate);
                    OracleParameter phoneNumber = new OracleParameter("p_telefon", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(phoneNumber);
                    OracleParameter email = new OracleParameter("p_email", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(email);
                    OracleParameter addressId = new OracleParameter("p_adresa_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(addressId);

                    comm.ExecuteNonQuery();

                    // var adresa = new AddressController().Get(addressId.Value.ToString());


                    result = new Customer()
                    {
                        ID = int.Parse(id),
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        BirthDate = Convert.ToDateTime(birthDate.Value),
                        PhoneNumber = phoneNumber.Value.ToString(),
                        Email = email.Value.ToString()
                    };
                }
            }

            return result;
        }

        public override List<Customer> GetAll()
		{
			List<Customer> result = new List<Customer>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select id_zakaznik, jmeno, prijmeni, datum_narozeni, telefon, email, adresa_id from zakaznici";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            // Address address = new AddressController().Get("1");
                            result.Add(new Customer()
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                BirthDate = rdr.GetDateTime(3),
                                PhoneNumber = rdr.GetString(4),
                                Email = rdr.GetString(5)
                            });
                        }
                    }
                }

            }

            return result;
        }

        public override Customer? Update(Customer item)
        {
            Customer? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_zakaznika";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_zakaznik", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = item.BirthDate;
                    comm.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = item.PhoneNumber;
                    comm.Parameters.Add("p_email", OracleDbType.Varchar2).Value = item.Email;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
