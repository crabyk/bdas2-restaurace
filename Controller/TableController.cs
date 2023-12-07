using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class TableController : Controller<Table>
    {
        public override Table? Add(Table item)
        {
            Table? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_stul";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_cislo_stolu", OracleDbType.Int32).Value = item.Number;
                    comm.Parameters.Add("p_id_stul", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_stul"].Value).Value;
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
                    comm.CommandText = "smazat_stul";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_stul", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Table? Get(string id)
        {
            Table? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_stul";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_stul", id);

                    OracleParameter number = new OracleParameter("p_cislo_stolu", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(number);

                    comm.ExecuteNonQuery();

                    result = new Table()
                    {
                        ID = int.Parse(id),
                        Number = int.Parse(number.Value.ToString())
                    };
                }
            }

            return result;
        }

        public override List<Table> GetAll()
        {
            List<Table> result = new List<Table>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_stoly";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new Table
                            {
                                ID = rdr.GetInt32(0),
                                Number = rdr.GetInt32(1)
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override Table? Update(Table item)
        {
            Table? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_stul";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_stul", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_cislo_stolu", OracleDbType.Int32).Value = item.Number;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
