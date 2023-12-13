using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class DrinkController : Controller<Drink>
    {

        public override Drink? Add(Drink item)
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

        // soft delete polozky
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

        // vraceni polozky z soft delete
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

        public override Drink? Get(string id)
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

                    OracleParameter name = new OracleParameter("p_nazev", OracleDbType.Varchar2, 64, null, ParameterDirection.Output);
                    comm.Parameters.Add(name);
                    OracleParameter price = new OracleParameter("p_cena", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(price);
                    OracleParameter volume = new OracleParameter("p_objem", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(volume);
                    OracleParameter available = new OracleParameter("p_dostupna", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(available);
                    OracleParameter imageId = new OracleParameter("p_id_obrazek", OracleDbType.Decimal, ParameterDirection.Output);
                    comm.Parameters.Add(imageId);
                    OracleParameter totalOrders = new OracleParameter("p_pocet_objednavek", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(totalOrders);

                    comm.ExecuteNonQuery();

                    var itemImage = new ItemImageController().Get(imageId.Value.ToString());

                    result = new Drink()
                    {
                        ID = int.Parse(id),
                        Name = name.Value.ToString(),
                        Price = double.Parse(price.Value.ToString()),
                        Volume = double.Parse(volume.Value.ToString()),
                        ItemImage = itemImage,
                        Available = int.Parse(available.Value.ToString()),
                        TotalOrders = int.Parse(totalOrders.Value.ToString())
                    };
                }
            }

            return result;
        }

        public override List<Drink> GetAll()
        {
            List<Drink> result = new List<Drink>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_napoje";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var itemImage = new ItemImageController().Get(rdr.GetInt32(5).ToString());
                            result.Add(new Drink
                            {
                                ID = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Price = rdr.GetInt32(2),
                                Volume = rdr.GetInt32(3),
                                Available = rdr.GetInt32(4),
                                ItemImage = itemImage,
                                TotalOrders = rdr.GetInt32(6)
                            });
                        }
                    }

                    return result;
                }
            }
        }

        public List<Drink> GetDrinkMenu()
        {
            List<Drink> result = new List<Drink>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_napojovy_list";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var itemImage = new ItemImageController().Get(rdr.GetInt32(4).ToString());
                            result.Add(new Drink
                            {
                                ID = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Price = rdr.GetInt32(2),
                                Volume = rdr.GetInt32(3),
                                ItemImage = itemImage,
                                Available = 1,
                                TotalOrders = rdr.GetInt32(5)
                            });
                        }
                    }

                    return result;
                }
            }
        }

        public override Drink? Update(Drink item)
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
                    comm.Parameters.Add("p_id_obrazek", OracleDbType.Decimal).Value = item.ItemImage.ID;
                    comm.Parameters.Add("p_pocet_objednavek", OracleDbType.Int32).Value = item.TotalOrders;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

        public Drink? GetMostOrderedDrink()
        {
            Drink? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "nejvice_objednavana_polozka";
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.BindByName = true;

                    comm.Parameters.Add("p_typ_polozky", OracleDbType.Varchar2).Value = "napoj";
                    comm.Parameters.Add("v_id_polozky", OracleDbType.Decimal, ParameterDirection.ReturnValue);

                    comm.ExecuteNonQuery();

                    var itemId = Convert.ToInt32(((OracleDecimal)comm.Parameters["v_id_polozky"].Value).Value);

                    result = Get(itemId.ToString());
                }
            }

            return result;
        }
    }
}
