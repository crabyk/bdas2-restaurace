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

                    OracleParameter name = new OracleParameter("p_nazev", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(name);
                    OracleParameter price = new OracleParameter("p_cena", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(price);
                    OracleParameter weight = new OracleParameter("p_hmotnost", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(weight);
                    OracleParameter recipe = new OracleParameter("p_recept", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(recipe);

                    comm.ExecuteNonQuery();

                    result = new Food()
                    {
                        ID = int.Parse(id),
                        Name = name.Value.ToString(),
                        Price = double.Parse(price.Value.ToString()),
                        Weight = double.Parse(weight.Value.ToString()),
                        Recipe = recipe.Value.ToString()
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
                string sql = "select j.id_polozka, nazev, cena, hmotnost, recept from polozky p join jidla j on p.id_polozka = j.id_polozka";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            // result.Add(new Food(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetString(4)));
                            result.Add(
                                new Food
                                {
                                    ID = rdr.GetInt32(0),
                                    Name = rdr.GetString(1),
                                    Price = rdr.GetInt32(2),
                                    Weight = rdr.GetInt32(3),
                                    Recipe = rdr.GetString(4),
                                });
                        }
                    }
                }
            }

            return result;
        }

        public List<Food> GetFoodFromView()
        {
            List<Food> food = new List<Food>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select id_polozka, nazev, cena, hmotnost, obrazek from v_jidelnicek";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int id = rdr.GetInt32(0);
                            string name = rdr.GetString(1);
                            int price = rdr.GetInt32(2);
                            int weight = rdr.GetInt32(3);

                            // ziskani obrazku ze sloupce typu blob
                            OracleBlob imgBlob = rdr.GetOracleBlob(4);
                            Byte[] byteArr = new Byte[imgBlob.Length];
                            int i = imgBlob.Read(byteArr, 0, Convert.ToInt32(imgBlob.Length));
                            MemoryStream memStream = new MemoryStream(byteArr);
                            var image = Image.FromStream(memStream);

                            food.Add(
                                new Food
                                {
                                    ID = id,
                                    Name = name,
                                    Price = price,
                                    Weight = weight,
                                    Recipe = string.Empty,
                                    Image = ConvertToBitmap(image)
                                });
                        }
                    }
                }
            }

            return food;
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

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

        public BitmapImage ConvertToBitmap(Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
