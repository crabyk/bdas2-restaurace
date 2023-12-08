using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class FoodController : Controller<Food>
    {

        public override Food? Add(Food item)
        {
            Food? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_jidlo";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;
                    comm.Parameters.Add("p_cena", OracleDbType.Decimal).Value = item.Price;
                    comm.Parameters.Add("p_hmotnost", OracleDbType.Decimal).Value = item.Weight;
                    comm.Parameters.Add("p_recept", OracleDbType.Varchar2).Value = item.Recipe;
                    comm.Parameters.Add("p_id_obrazek", OracleDbType.Decimal).Value = item.ItemImage.ID;
                    comm.Parameters.Add("p_id_polozka", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_polozka"].Value).Value;
                }

                result = item;
                result.ID = Convert.ToInt32(newId);
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
                    comm.CommandText = "smazat_polozku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public int Cancel(string id)
        {
            int result = 0;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "zrusit_polozku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public int Restore(string id)
        {
            int result = 0;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "obnovit_polozku";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Food? Get(string id)
        {
            Food? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_jidlo";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", id);

                    OracleParameter name = new OracleParameter("p_nazev", OracleDbType.Varchar2, 64, null, ParameterDirection.Output);
                    comm.Parameters.Add(name);
                    OracleParameter price = new OracleParameter("p_cena", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(price);
                    OracleParameter weight = new OracleParameter("p_hmotnost", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(weight);
                    OracleParameter recipe = new OracleParameter("p_recept", OracleDbType.Varchar2, 2048, null, ParameterDirection.Output);
                    comm.Parameters.Add(recipe);
                    OracleParameter imageId = new OracleParameter("p_id_obrazek", OracleDbType.Decimal, ParameterDirection.Output);
                    comm.Parameters.Add(imageId);

                    comm.ExecuteNonQuery();

                    var itemImage = new ItemImageController().Get(imageId.Value.ToString());

                    result = new Food()
                    {
                        ID = int.Parse(id),
                        Name = name.Value.ToString(),
                        Price = double.Parse(price.Value.ToString()),
                        Weight = double.Parse(weight.Value.ToString()),
                        Recipe = recipe.Value.ToString(),
                        ItemImage = itemImage
                    };
                }
            }

            return result;
        }

        public override List<Food> GetAll()
        {
            List<Food> result = new List<Food>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_jidla";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var itemImage = new ItemImageController().Get(rdr.GetInt32(5).ToString());
                            result.Add(new Food
                            {
                                ID = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Price = rdr.GetInt32(2),
                                Weight = rdr.GetInt32(3),
                                Recipe = rdr.GetString(4),
                                ItemImage = itemImage
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override Food? Update(Food item)
        {
            Food? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_jidlo";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_polozka", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;
                    comm.Parameters.Add("p_cena", OracleDbType.Int32).Value = item.Price;
                    comm.Parameters.Add("p_hmotnost", OracleDbType.Int32).Value = item.Weight;
                    comm.Parameters.Add("p_recept", OracleDbType.Varchar2).Value = item.Recipe;
                    comm.Parameters.Add("p_id_obrazek", OracleDbType.Decimal).Value = item.ItemImage.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
