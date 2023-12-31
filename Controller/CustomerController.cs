﻿using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class CustomerController : Controller<Customer>
    {

        public Customer? GetByUser(User user)
        {
            Customer? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "select id_zakaznik, jmeno, prijmeni, datum_narozeni, telefon, email, adresa_id FROM zakaznici WHERE id_uzivatel = :uzivatelId";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Clear();
                    comm.Parameters.Add(":uzivatelId", OracleDbType.Int32).Value = user.ID;

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var address = new AddressController().Get(rdr.GetString(6));

                            result = new Customer()
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                BirthDate = rdr.GetDateTime(3),
                                PhoneNumber = rdr.GetString(4),
                                Email = rdr.GetString(5),
                                Address = address,
                                User = user
                            };
                        }
                    }
                }

            }

            return result;
        }

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
                    comm.Parameters.Add("p_uzivatel_id", OracleDbType.Decimal).Value = item.User == null ? null : item.User.ID;
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

                    OracleParameter firstName = new OracleParameter("p_jmeno", OracleDbType.Varchar2, 20, null, ParameterDirection.Output);
                    comm.Parameters.Add(firstName);
                    OracleParameter lastName = new OracleParameter("p_prijmeni", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    comm.Parameters.Add(lastName);
                    OracleParameter birthDate = new OracleParameter("p_datum_narozeni", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(birthDate);
                    OracleParameter phoneNumber = new OracleParameter("p_telefon", OracleDbType.Varchar2, 12, null, ParameterDirection.Output);
                    comm.Parameters.Add(phoneNumber);
                    OracleParameter email = new OracleParameter("p_email", OracleDbType.Varchar2, 128, null, ParameterDirection.Output);
                    comm.Parameters.Add(email);
                    OracleParameter addressId = new OracleParameter("p_adresa_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(addressId);
                    OracleParameter userId = new OracleParameter("p_id_uzivatel", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(userId);

                    comm.ExecuteNonQuery();

                    var adresa = new AddressController().Get(addressId.Value.ToString());

                    User user = new UserController().Get(userId.Value.ToString());


                    result = new Customer()
                    {
                        ID = int.Parse(id),
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        BirthDate = ((OracleDate)birthDate.Value).Value,
                        PhoneNumber = phoneNumber.Value.ToString(),
                        Email = email.Value.ToString(),
                        Address = adresa,
                        User = user
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
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_zakazniky";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var address = new AddressController().Get(rdr.GetString(6));

                            User user = new User();
                            if (!rdr.IsDBNull(7))
                                user = new UserController().Get(rdr.GetString(7));

                            result.Add(new Customer()
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                BirthDate = rdr.GetDateTime(3),
                                PhoneNumber = rdr.GetString(4),
                                Email = rdr.GetString(5),
                                Address = address,
                                User = user
                            });
                        }
                    }
                }
            }

            return result;
        }

        public List<Customer> GetAllRespectAnonymity()
        {
            List<Customer> result = new List<Customer>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_zakazniky_s_anonymizaci";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var address = new AddressController().Get(rdr.GetString(6));

                            User user = new User();
                            if (!rdr.IsDBNull(7))
                                user = new UserController().Get(rdr.GetString(7));

                            result.Add(new Customer()
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                BirthDate = rdr.GetDateTime(3),
                                PhoneNumber = rdr.GetString(4),
                                Email = rdr.GetString(5),
                                Address = address,
                                User = user
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
