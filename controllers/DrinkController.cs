using BDAS2_Restaurace.db;
using BDAS2_Restaurace.models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.controllers
{
	 class DrinkController : IController<Drink>
	{
		static OracleConnection conn = Database.Connect();


		static public Drink Add(Drink item)
		{
			throw new NotImplementedException();
		}

		static public int Delete(string id)
		{
			throw new NotImplementedException();
		}

		static public Drink Get(string id)
		{
			throw new NotImplementedException();
		}

		static public List<Drink> GetAll()
		{
			List<Drink> result = new List<Drink>();

			using (conn)
			{
				conn.Open();
				string sql = "select nazev, cena, objem from polozky p join napoje n on p.id_polozka = n.id_polozka";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							result.Add(new Drink(rdr.GetString(0), rdr.GetInt32(1), rdr.GetInt32(2)));
						}
					}
				}
			}

			return result;
		}

		static public Drink Update(Drink item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
