using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace BDAS2_Restaurace.Controller
{
    public class ReservationController : Controller<Reservation>
    {


        public override Reservation? Add(Reservation item)
        {
            Reservation? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;
                // transakce k zajisteni konzistence dat
                OracleTransaction orderAddTransaction = null;

                try
                {
                    orderAddTransaction = conn.BeginTransaction();

                    using (OracleCommand comm = conn.CreateCommand())
                    {
                        comm.Transaction = orderAddTransaction;


                        Customer customer = item.Customer;
                        if (customer.ID == null || customer.User == null)
                        {
                            customer = new CustomerController().Add(item.Customer);
                            Address address = new AddressController().Add(item.Customer.Address);
                            customer.Address.ID = address.ID;
                        }

                        Table table = new Table();
                        table.ID = item.Table.ID;

                        comm.CommandText = "vlozit_rezervaci";
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.Add("p_cas_rezervace", OracleDbType.Date).Value = item.ReservationDate;
                        comm.Parameters.Add("p_pocet_osob", OracleDbType.Int32).Value = item.NumberOfPeople;
                        comm.Parameters.Add("p_zakaznik_id", OracleDbType.Int32).Value = item.Customer.ID;
                        comm.Parameters.Add("p_stul_id", OracleDbType.Int32).Value = item.Table.ID;
                        comm.Parameters.Add("p_id_rezervace", OracleDbType.Decimal, ParameterDirection.Output);

                        comm.ExecuteNonQuery();

                        newId = ((OracleDecimal)comm.Parameters["p_id_rezervace"].Value).Value;

                        comm.ExecuteNonQuery();
                        newId = ((OracleDecimal)comm.Parameters["p_id_objednavka"].Value).Value;
                        item.ID = Convert.ToInt32(newId);

                    }
                    orderAddTransaction.Commit();
                    result = item;
                }
                catch (Exception ex)
                {
                    orderAddTransaction?.Rollback();
                    throw ex;
                }
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
                    comm.CommandText = "smazat_rezervaci";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_rezervace", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Reservation? Get(string id)
        {
            Reservation? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_rezervaci";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_rezervace", id);

                    OracleParameter reservationDate = new OracleParameter("p_cas_rezervace", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(reservationDate);
                    OracleParameter numberOfPeople = new OracleParameter("p_pocet_osob", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(numberOfPeople);
                    OracleParameter zakaznikId = new OracleParameter("p_zakaznik_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(zakaznikId);
                    OracleParameter stulId = new OracleParameter("p_stul_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(stulId);

                    comm.ExecuteNonQuery();

                    var customer = new CustomerController().Get(zakaznikId.Value.ToString());
                    var table = new TableController().Get(stulId.Value.ToString());

                    result = new Reservation()
                    {
                        ID = int.Parse(id),
                        ReservationDate = ((OracleDate)reservationDate.Value).Value,
                        NumberOfPeople = int.Parse(numberOfPeople.Value.ToString()),
                        Customer = customer,
                        Table = table
                    };
                }
            }

            return result;
        }

        public override List<Reservation> GetAll()
        {
            List<Reservation> result = new List<Reservation>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_rezervace";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var customer = new CustomerController().Get(rdr.GetInt32(3).ToString());
                            var table = new TableController().Get(rdr.GetInt32(4).ToString());

                            result.Add(new Reservation
                            {
                                ID = rdr.GetInt32(0),
                                ReservationDate = rdr.GetDateTime(1),
                                NumberOfPeople = rdr.GetInt32(2),
                                Customer = customer,
                                Table = table
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override Reservation? Update(Reservation item)
        {
            Reservation? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_rezervaci";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_rezervace", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_cas_rezervace", OracleDbType.Date).Value = item.ReservationDate;
                    comm.Parameters.Add("p_pocet_osob", OracleDbType.Int32).Value = item.NumberOfPeople;
                    comm.Parameters.Add("p_zakaznik_id", OracleDbType.Int32).Value = item.Customer.ID;
                    comm.Parameters.Add("p_stul_id", OracleDbType.Int32).Value = item.Table.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
