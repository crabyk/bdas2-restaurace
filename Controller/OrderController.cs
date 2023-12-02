using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    class OrderController
    {
        static public Order? Add(Order item)
        {
            Order? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal orderId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add(":p_datum", OracleDbType.Date).Value = item.OrderDate;
                    comm.Parameters.Add(":p_platba_id", OracleDbType.Decimal).Value = item.Payment.ID;
                    comm.Parameters.Add(":p_zakaznik_id", OracleDbType.Decimal).Value = item.Customer.ID;     
                    // TODO osetrit kdyz nebude stul nebo adresa
                    comm.Parameters.Add(":p_stul_id", OracleDbType.Decimal).Value = item.Table.ID;
                    comm.Parameters.Add(":p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;

                    OracleParameter p = new OracleParameter(":p_id_objednavka", OracleDbType.Decimal, ParameterDirection.Output);
                    comm.Parameters.Add(p);

                    comm.ExecuteNonQuery();

                    orderId = ((OracleDecimal)p.Value).Value;
                    item.ID = Convert.ToInt32(orderId);
                }

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "insert into polozky_objednavky (objednavka_id, polozka_id) VALUES (:objednavkaId, :polozkaId)";

                    foreach (Item orderItem in item.Items)
                    {
                        comm.Parameters.Add(":objednavkaId", OracleDbType.Decimal).Value = item.ID;
                        comm.Parameters.Add(":polozkaId", OracleDbType.Decimal).Value = orderItem.ID;
                        comm.ExecuteNonQuery();
                    }
                }

                result = item;
            }

            return result;
        }

        static public int Delete(string id)
        {
            int result = 0;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "smazat_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add(":p_id_objednavka", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        static public Order? Get(string id)
        {
            Order? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add(":p_id_objednavka", id);

                    OracleParameter orderDate = new OracleParameter(":p_datum", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(orderDate);
                    OracleParameter paymentId = new OracleParameter(":p_platba_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(paymentId);
                    OracleParameter customerId = new OracleParameter(":p_zakaznik_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(customerId);
                    OracleParameter tableId = new OracleParameter(":p_stul_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(tableId);
                    OracleParameter addressId = new OracleParameter(":p_adresa_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(addressId);

                    comm.ExecuteNonQuery();

                    var payment = PaymentController.Get(paymentId.Value.ToString());
                    var customer = CustomerController.Get(customerId.Value.ToString());

                    // TODO dodelat vytvoreni objektu objednavky
                    // udelat metody k zjisteni vsech polozek pro danou objednavku
                    // dal zjistit stul nebo adresu podle objednavky
                    result = new Order()
                    {
                        ID = int.Parse(id),
                        OrderDate = ((OracleDate)orderDate.Value).Value,
                        Payment = payment,
                        Customer = customer
                        // polozky, stul/adresa
                    };
                }

            }

            return result;
        }

        static public List<Order> GetAll()
        {
            List<Order> result = new List<Order>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select id_adresa, ulice, mesto, cislo_popisne, psc, stat from adresy";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            //result.Add(new Order(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5)));
                        }
                    }
                }

            }

            return result;
        }

        static public Order? Update(Order item)
        {
            Order? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add(":p_id_objednavka", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add(":p_datum", OracleDbType.Date).Value = item.OrderDate;
                    comm.Parameters.Add(":p_platba_id", OracleDbType.Int32).Value = item.Payment.ID;
                    comm.Parameters.Add(":p_zakaznik_id", OracleDbType.Int32).Value = item.Customer.ID;
                    // TODO predat bud pouze stul nebo adresu podle toho, co objednavka obsahuje
                    comm.Parameters.Add(":p_stul_id", OracleDbType.Int32).Value = item.Table.ID;
                    comm.Parameters.Add(":p_adresa_id", OracleDbType.Int32).Value = item.Address.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
