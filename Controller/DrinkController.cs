using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    class DrinkController
    {

        static public Drink? Add(Drink item)
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

        static public int Delete(string id)
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

		static public Drink? Get(string id)
		{
			Drink? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select j.id_polozka, nazev, cena, objem from polozky p join jidla n on p.id_polozka = n.id_polozka where n.id_polozka = :id";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					comm.Parameters.Add(":id", id);

					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							// result = new Drink(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3));
						}
					}
				}

			}

            return result;
        }

        static public List<Drink> GetAll()
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
                }
            }

            return result;
        }

		static public Drink? Update(Drink item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
