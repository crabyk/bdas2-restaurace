using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Controller
{
	class PaymentController
	{

		static public Payment? Add(Payment item)
		{
			Payment? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				decimal newId;

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into platby (suma, datum, typ_platba_id) VALUES (:suma, :datum, :typPlatbaId) returning id_platba into :newId";

					comm.Parameters.Add(":nazev", OracleDbType.Varchar2).Value = item.Amount;
					comm.Parameters.Add(":cena", OracleDbType.Decimal).Value = item.Date;
					comm.Parameters.Add(":typPlatbaId", OracleDbType.Decimal).Value = item.Type.ID;

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

		static public PaymentType? AddType(PaymentType item)
		{
			PaymentType? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				decimal newId;

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into typy_plateb (nazev) VALUES (:nazev) returning id_typ_platby into :newId";

					comm.Parameters.Add(":nazev", OracleDbType.Varchar2).Value = item.Name;

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

		static public Payment? Get(string id)
		{
			Payment? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select t.id_typ_platby, t.nazev, id_platba, suma, datum  from platby p join typy_plateb t on p.typ_platby_id = t.id_typ_platby where p.typ_platby_id = :id";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					comm.Parameters.Add(":id", id);

					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							PaymentType paymentType = new PaymentType
							{
								ID = rdr.GetInt32(0),
								Name = rdr.GetString(1) 
							};
							result = new Payment
							{
								ID = rdr.GetInt32(2),
								Amount = rdr.GetDouble(3),
								Date = rdr.GetDateTime(4),
								Type = paymentType	
							};
						}
					}
				}

			}

			return result;
		}

		static public List<Payment> GetAll()
		{
			List<Payment> result = new List<Payment>();

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select t.id_typ_platby, t.nazev, id_platba, suma, datum  from platby p join typy_plateb t on p.typ_platby_id = t.id_typ_platby";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							PaymentType paymentType = new PaymentType
							{
								ID = rdr.GetInt32(0),
								Name = rdr.GetString(1)
							};
							result.Add(new Payment
							{
								ID = rdr.GetInt32(2),
								Amount = rdr.GetDouble(3),
								Date = rdr.GetDateTime(4),
								Type = paymentType
							});
						}
					}
				}
			}

			return result;
		}

		static public List<PaymentType> GetAllTypes()
		{
			List<PaymentType> result = new List<PaymentType>();

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select id_typ_platby, nazev from typy_plateb";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							result.Add(new PaymentType
							{
								ID = rdr.GetInt32(0),
								Name = rdr.GetString(1)
							});
						}
					}
				}
			}

			return result;
		}


		static public Payment? Update(Payment item, string id)
		{
			throw new NotImplementedException();
		}


	}
}
