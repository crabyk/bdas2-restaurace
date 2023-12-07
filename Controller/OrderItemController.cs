using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class OrderItemController
    {
        public Item Add(Item item, Order order)
        {
            Item? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "insert into polozky_objednavky (objednavka_id, polozka_id) VALUES (:objednavkaId, :polozkaId)";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Clear();
                    comm.Parameters.Add(":objednavkaId", OracleDbType.Int32).Value = order.ID;
                    comm.Parameters.Add(":polozkaId", OracleDbType.Int32).Value = item.ID;
                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

        public Item Delete(Item item, Order order)
        {
            Item? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "delete from polozky_objednavky where objednavka_id = :objednavkaId and polozka_id = :polozkaId";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Clear();
                    comm.Parameters.Add(":objednavkaId", OracleDbType.Int32).Value = order.ID;
                    comm.Parameters.Add(":polozkaId", OracleDbType.Int32).Value = item.ID;
                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

        public List<Item> GetAll(string orderId)
        {
            List<Item> result = new List<Item>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = string.Empty;

                sql = "select n.id_polozka, nazev, cena, objem from polozky p " +
                        "join polozky_objednavky po on p.id_polozka = po.polozka_id " +
                        "join napoje n on p.id_polozka = n.id_polozka " +
                        "where objednavka_id = :id";

                using (OracleCommand comm = new OracleCommand(sql, conn))
                {

                    comm.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = orderId;

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(
                                new Drink
                                {
                                    ID = rdr.GetInt32(0),
                                    Name = rdr.GetString(1),
                                    Price = rdr.GetInt32(2),
                                    Volume = rdr.GetInt32(3)
                                });
                        }
                    }
                }

                sql = "select n.id_polozka, nazev, cena, hmotnost, recept from polozky p " +
                    "join polozky_objednavky po on p.id_polozka = po.polozka_id " +
                    "join jidla n on p.id_polozka = n.id_polozka " +
                    "where objednavka_id = :id";

                using (OracleCommand comm = new OracleCommand(sql, conn))
                {

                    comm.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = orderId;

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(
                                new Food
                                {
                                    ID = rdr.GetInt32(0),
                                    Name = rdr.GetString(1),
                                    Price = rdr.GetInt32(2),
                                    Weight = rdr.GetInt32(3),
                                    Recipe = rdr.GetString(4)
                                });
                        }
                    }
                }

            }

            return result;
        }
    }
}
