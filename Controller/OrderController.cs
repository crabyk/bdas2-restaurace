using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					comm.CommandText = "insert into objednavky (datum, platba_id, zakaznik_id, stul_id, adresa_id) VALUES (:datum, :platbaId, :zakaznikId, :stulId, :adresaId) returning id_objednavka into :newId";

					comm.Parameters.Add(":datum", OracleDbType.Date).Value = item.OrderDate;
					comm.Parameters.Add(":platbaId", OracleDbType.Int32).Value = item.Payment.ID;
					comm.Parameters.Add(":zakaznikId", OracleDbType.Int32).Value = item.Customer.ID;
					comm.Parameters.Add(":stulId", OracleDbType.Int32).Value = item.Table.ID;
					comm.Parameters.Add(":adresaId", OracleDbType.Int32).Value = item.Address.ID;

					OracleParameter p = new OracleParameter(":newId", OracleDbType.Decimal);
					p.Direction = ParameterDirection.Output;
					comm.Parameters.Add(p);

					comm.ExecuteNonQuery();

					orderId = ((OracleDecimal)p.Value).Value;

				}

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into polozky_objednavky (objednavka_id, polozka_id) VALUES (:objednavkaId, :polozkaId)";

					foreach (Item orderItem in item.Items)
					{
						comm.Parameters.Add(":objednavkaId", OracleDbType.Int32).Value = item.ID;
						comm.Parameters.Add(":polozkaId", OracleDbType.Int32).Value = orderItem.ID;
						comm.ExecuteNonQuery();
					}

				}

				result = item;
				result.ID = Convert.ToInt32(orderId);

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
					comm.CommandText = "delete from adresy where id_adresa = :id";

					comm.Parameters.Add(":id", id);

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
				string sql = "select id_adresa, ulice, mesto, cislo_popisne, psc, stat from adresy where id_adresa = :id";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					comm.Parameters.Add(":id", id);

					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							//result = new Order(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5));
						}
					}
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

		static public Order? Update(Order item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
