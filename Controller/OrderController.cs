using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace BDAS2_Restaurace.Controller
{
    public class OrderController : Controller<Order>
    {
        public override Order? Add(Order item)
        {
            Order? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal orderId;
                // transakce k zajisteni konzistence dat
                OracleTransaction orderAddTransaction = null;

                try
                {
                    orderAddTransaction = conn.BeginTransaction();

                    using (OracleCommand comm = conn.CreateCommand())
                    {
                        comm.Transaction = orderAddTransaction;

                        Address address = new AddressController().Add(item.Customer.Address);
                        item.Customer.Address.ID = address.ID;
                        Customer customer = new CustomerController().Add(item.Customer);

                        Payment payment = new PaymentController().Add(item.Payment);

                        Table table = new Table();
                        table.ID = 1;

                        comm.CommandText = "vlozit_objednavku";
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.Add("p_datum", OracleDbType.Date).Value = item.OrderDate;
                        comm.Parameters.Add("p_platba_id", OracleDbType.Decimal).Value = payment.ID;
                        comm.Parameters.Add("p_zakaznik_id", OracleDbType.Decimal).Value = customer.ID;
                        comm.Parameters.Add("p_stul_id", OracleDbType.Decimal).Value = item.Table?.ID;
                        comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = address.ID;
                        comm.Parameters.Add("p_id_objednavka", OracleDbType.Decimal, ParameterDirection.Output);

                        comm.ExecuteNonQuery();
                        orderId = ((OracleDecimal)comm.Parameters["p_id_objednavka"].Value).Value;
                        item.ID = Convert.ToInt32(orderId);

                        comm.CommandText = "insert into polozky_objednavky (objednavka_id, polozka_id) VALUES (:objednavkaId, :polozkaId)";
                        comm.CommandType = CommandType.Text;

                        foreach (Item orderItem in item.Items)
                        {
                            comm.Parameters.Clear();
                            comm.Parameters.Add(":objednavkaId", OracleDbType.Decimal).Value = item.ID;
                            comm.Parameters.Add(":polozkaId", OracleDbType.Decimal).Value = orderItem.ID;
                            comm.ExecuteNonQuery();
                        }
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
                    comm.CommandText = "smazat_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_objednavka", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Order? Get(string id)
        {
            Order? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_objednavka", id);

                    OracleParameter orderDate = new OracleParameter("p_datum", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(orderDate);
                    OracleParameter paymentId = new OracleParameter("p_platba_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(paymentId);
                    OracleParameter customerId = new OracleParameter("p_zakaznik_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(customerId);
                    OracleParameter tableId = new OracleParameter("p_stul_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(tableId);
                    OracleParameter addressId = new OracleParameter("p_adresa_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(addressId);

                    comm.ExecuteNonQuery();

                    var payment = new PaymentController().Get(paymentId.Value.ToString());
                    var customer = new CustomerController().Get(customerId.Value.ToString());

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

        public override List<Order> GetAll()
        {
            List<Order> result = new List<Order>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_objednavky";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Payment payment = new PaymentController().Get(rdr.GetInt32(2).ToString());
                            Customer customer = new CustomerController().Get(rdr.GetInt32(3).ToString());
                            Table? table = null;
                            if (!rdr.IsDBNull(4))
                                table = new TableController().Get(rdr.GetInt32(4).ToString());

                            Address? address = null;
                            if (!rdr.IsDBNull(5))
                                address = new AddressController().Get(rdr.GetInt32(5).ToString());

                            List<Item> items = new OrderItemController().GetAll(rdr.GetInt32(0).ToString());

                            result.Add(new Order()
                            {
                                ID = rdr.GetInt32(0),
                                OrderDate = rdr.GetDateTime(1),
                                Payment = payment,
                                Customer = customer,
                                Items = new ObservableCollection<Item>(items),
                                Table = table,
                                Address = address
                            });
                        }
                    }
                }

            }

            return result;
        }

        public override Order? Update(Order item)
        {
            Order? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    new PaymentController().Update(item.Payment);

                    // Pridavani funguje, ale odebirani nejak moc ne

                    List<Item> orderItems = new OrderItemController().GetAll(item.ID.ToString());
                    List<Item> newOrderItems = new List<Item>(item.Items);

                    List<Item> itemsAdd = newOrderItems.Where(i1 => !orderItems.Any(i2 => i2.ID == i1.ID)).ToList();
                    List<Item> itemsRemove = orderItems.Where(i1 => !newOrderItems.Any(i2 => i2.ID == i1.ID)).ToList();

                    itemsAdd.ForEach(i => new OrderItemController().Add(i, item));
                    itemsRemove.ForEach(i => new OrderItemController().Delete(i, item));



                    comm.CommandText = "upravit_objednavku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_objednavka", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_datum", OracleDbType.Date).Value = item.OrderDate;
                    comm.Parameters.Add("p_platba_id", OracleDbType.Int32).Value = item.Payment.ID;
                    comm.Parameters.Add("p_zakaznik_id", OracleDbType.Int32).Value = item.Customer.ID;
                    comm.Parameters.Add("p_stul_id", OracleDbType.Int32).Value = item.Table?.ID;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Int32).Value = item.Address?.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
