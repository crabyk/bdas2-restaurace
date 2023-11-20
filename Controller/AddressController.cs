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
	class AddressController
	{

		static public Address? Add(Address item)
		{
			Address? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				decimal newId;

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into adresy (ulice, mesto, cislo_popisne, psc, stat) VALUES (:ulice, :mesto, :cisloPopisen, :psc, :stat) returning id_adresa into :newId";

					comm.Parameters.Add(":ulice", OracleDbType.Varchar2).Value = item.StreetName;
					comm.Parameters.Add(":mesto", OracleDbType.Varchar2).Value = item.CityName;
					comm.Parameters.Add(":cisloPopisne", OracleDbType.Varchar2).Value = item.UnitNumber;
					comm.Parameters.Add(":psc", OracleDbType.Varchar2).Value = item.PostalCode;
					comm.Parameters.Add(":stat", OracleDbType.Varchar2).Value = item.Country;

					OracleParameter p = new OracleParameter(":newId", OracleDbType.Decimal);
					p.Direction = ParameterDirection.Output;
					comm.Parameters.Add(p);

					comm.ExecuteNonQuery();

					newId = ((OracleDecimal)p.Value).Value;

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
					comm.CommandText = "delete from adresy where id_adresa = :id";

					comm.Parameters.Add(":id", id);

					result = comm.ExecuteNonQuery();
				}
			}

			return result;
		}

		static public Address? Get(string id)
		{
			Address? result = null;

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
							result = new Address(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5));
						}
					}
				}

			}

			return result;
		}

		static public List<Address> GetAll()
		{
			List<Address> result = new List<Address>();

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
							result.Add(new Address(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5)));
						}
					}
				}

			}

			return result;
		}

		static public Address? Update(Address item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
