﻿using BDAS2_Restaurace.db;
using BDAS2_Restaurace.models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.controllers
{
	 class FoodController : IController<Food>
	{

		static public Food? Add(Food item)
		{
			Food? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				decimal polozkaId;

				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into polozky (nazev, cena, typ_polozky) VALUES (:nazev, :cena, :typPolozky) returning id_polozka into :polozkaId";
					//comm.Parameters.Add(":polozkaId", OracleDbType.Decimal).Direction = ParameterDirection.ReturnValue;


					comm.Parameters.Add(":nazev", OracleDbType.Varchar2).Value = item.Name;
					comm.Parameters.Add(":cena", OracleDbType.Decimal).Value = item.Price;
					comm.Parameters.Add(":typPolozky", OracleDbType.Varchar2).Value = "jidlo";
					OracleParameter p = new OracleParameter(":polozkaId", OracleDbType.Decimal);
					p.Direction = ParameterDirection.Output;
					comm.Parameters.Add(p);

					comm.ExecuteNonQuery();


					polozkaId = ((OracleDecimal)p.Value).Value;

				}
				
				using (OracleCommand comm = conn.CreateCommand())
				{
					comm.CommandText = "insert into jidla (hmotnost, recept, id_polozka) VALUES (:hmotnost, :recept, :polozkaId)";
					
					comm.Parameters.Add(":hmotnost", item.Weight);
					comm.Parameters.Add(":recept", item.Recipe);
					comm.Parameters.Add(":polozkaId", polozkaId);

					int rowsAffected = comm.ExecuteNonQuery();
				}

				result = item;
				result.ID = Convert.ToInt32(polozkaId);
				
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

		static public Food? Get(string id)
		{
			Food? result = null;

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select j.id_polozka, nazev, cena, hmotnost, recept from polozky p join jidla j on p.id_polozka = j.id_polozka";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					comm.Parameters.Add(":id", id);

					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							result = new Food(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetString(4));
						}
					}
				}

			}

			return result;
		}

		static public List<Food> GetAll()
		{
			List<Food> result = new List<Food>();

			using (OracleConnection conn = Database.Connect())
			{
				conn.Open();
				string sql = "select j.id_polozka, nazev, cena, hmotnost, recept from polozky p join jidla j on p.id_polozka = j.id_polozka";
				using (OracleCommand comm = new OracleCommand(sql, conn))
				{
					using (OracleDataReader rdr = comm.ExecuteReader())
					{
						while (rdr.Read())
						{
							result.Add(new Food(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetString(4)));
						}
					}
				}
			}

			return result;
		}

		static public Food? Update(Food item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
