using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    class DrinkController : Controller<Drink>
    {

        public new Drink? Add(Drink item)
        {
            Drink? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_napoj";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;
                    comm.Parameters.Add("p_cena", OracleDbType.Int32).Value = item.Price;
                    comm.Parameters.Add("p_objem", OracleDbType.Int32).Value = item.Volume;
                    comm.Parameters.Add("p_id_polozka", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_polozka"].Value).Value;
                }

                result = item;
                result.ID = Convert.ToInt32(newId);
            }

            return result;
        }

        public new int Delete(string id)
        {
            int result = 0;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "smazat_polozku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }


        public new Drink? Get(int id)
        {
            Drink? result = null;


            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_napoj";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", id);

                    OracleParameter name = new OracleParameter("p_nazev", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(name);
                    OracleParameter price = new OracleParameter("p_cena", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(price);
                    OracleParameter volume = new OracleParameter("p_objem", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(volume);

                    comm.ExecuteNonQuery();

                    result = new Drink()
                    {
                        ID = id,
                        Name = name.Value.ToString(),
                        Price = double.Parse(price.Value.ToString()),
                        Volume = double.Parse(volume.Value.ToString())
                    };
                }
            }

            return result;
        }

        public new List<Drink> GetAll()
        {
            List<Drink> result = new List<Drink>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select n.id_polozka, nazev, cena, objem from polozky p join napoje n on p.id_polozka = n.id_polozka";

                using (OracleCommand comm = new OracleCommand(sql, conn))
                {

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            // result.Add(new Drink(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3)));
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

                    return result;
                }
            }
        }


        public new Drink? Update(Drink item)
        {
            Drink? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_napoj";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;
                    comm.Parameters.Add("p_cena", OracleDbType.Int32).Value = item.Price;
                    comm.Parameters.Add("p_objem", OracleDbType.Int32).Value = item.Volume;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
