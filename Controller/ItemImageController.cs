using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class ItemImageController : Controller<ItemImage>
    {
        public override ItemImage? Add(ItemImage item)
        {
            ItemImage? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;
                var imageBlob = DbUtils.ConvertToBlobFromBitmap(item.Image, conn);

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_obrazek";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_nazev_souboru", OracleDbType.Varchar2).Value = item.FileName;
                    comm.Parameters.Add("p_datum_modifikace", OracleDbType.Date).Value = item.ModifyDate;
                    comm.Parameters.Add("p_binarni_obsah", OracleDbType.Blob).Value = imageBlob;
                    comm.Parameters.Add("p_id_obrazek", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_obrazek"].Value).Value;
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
                    comm.CommandText = "smazat_obrazek";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_obrazek", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override ItemImage? Get(string id)
        {
            ItemImage? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_obrazek";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_obrazek", id);

                    OracleParameter fileName = new OracleParameter("p_nazev_souboru", OracleDbType.Varchar2, 255, null, ParameterDirection.Output);
                    comm.Parameters.Add(fileName);
                    OracleParameter modifyDate = new OracleParameter("p_datum_modifikace", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(modifyDate);
                    OracleParameter imgBlob = new OracleParameter("p_binarni_obsah", OracleDbType.Blob, ParameterDirection.Output);
                    comm.Parameters.Add(imgBlob);

                    comm.ExecuteNonQuery();

                    var image = DbUtils.ConvertToBitmapFromBlob((OracleBlob)imgBlob.Value);

                    result = new ItemImage()
                    {
                        ID = int.Parse(id),
                        FileName = fileName.Value.ToString(),
                        ModifyDate = ((OracleDate)modifyDate.Value).Value,
                        Image = image
                    };
                }
            }

            return result;
        }

        public override List<ItemImage> GetAll()
        {
            List<ItemImage> result = new List<ItemImage>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_obrazky";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var image = DbUtils.ConvertToBitmapFromBlob(rdr.GetOracleBlob(3));
                            result.Add(new ItemImage
                            {
                                ID = rdr.GetInt32(0),
                                FileName = rdr.GetString(1),
                                ModifyDate = rdr.GetDateTime(2),
                                Image = image
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override ItemImage? Update(ItemImage item)
        {
            ItemImage? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                var imageBlob = DbUtils.ConvertToBlobFromBitmap(item.Image, conn);

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_obrazek";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_obrazek", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_nazev_souboru", OracleDbType.Varchar2).Value = item.FileName;
                    comm.Parameters.Add("p_datum_modifikace", OracleDbType.Date).Value = item.ModifyDate;
                    comm.Parameters.Add("p_binarni_obsah", OracleDbType.Blob).Value = imageBlob;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
