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
	public class CustomerController : Controller<Customer>
	{

		public static new Customer? Add(Customer item)
		{
			Customer? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				decimal newId;

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into zakaznici (jmeno, prijmeni, datum_narozeni, telefon, email, adresa_id) VALUES (:jmeno, :prijmeni, :datumNarozeni, :telefon, :email, :adresaId) returning id_zakaznik into :newId";

					comm.Parameters.Add(":jmeno", OracleDbType.Varchar2).Value = item.FirstName;
					comm.Parameters.Add(":prijmeni", OracleDbType.Varchar2).Value = item.LastName;
					comm.Parameters.Add(":datumNarozeni", OracleDbType.Date).Value = item.BirthDate;
					comm.Parameters.Add(":telefon", OracleDbType.Varchar2).Value = item.PhoneNumber;
					comm.Parameters.Add(":email", OracleDbType.Varchar2).Value = item.Email;
					comm.Parameters.Add(":adresaId", OracleDbType.Decimal).Value = item.Address.ID;


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

		static public new int Delete(string id)
		{
			int result = 0;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "delete from jidla where id_polozka = :id";

					comm.Parameters.Add(":id", id);

					result = comm.ExecuteNonQuery();
				}

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "delete from polozky where id_polozka = :id";

					comm.Parameters.Add(":id", id);

					result = comm.ExecuteNonQuery();
				}
			}

			return result;
		}

		static public new Customer? Get(string id)
		{
			Customer? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select id_zakaznik, jmeno, prijmeni, datum_narozeni, telefon, email, adresa_id from zakaznici where id_zakaznik = :id";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					comm.Parameters.Add(":id", id);

					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							// result = new Customer(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetString(5), AddressController.Get(rdr.GetString(6)));
						}
					}
				}

			}

			return result;
		}

		static public new List<Customer> GetAll()
		{
			List<Customer> result = new List<Customer>();

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select id_zakaznik, jmeno, prijmeni, datum_narozeni, telefon, email, adresa_id from zakaznici";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{

					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							// result.Add(new Customer(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetString(5), AddressController.Get(rdr.GetString(6))));
						}
					}
				}

			}

			return result;
		}

		static public Customer? Update(Customer item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
