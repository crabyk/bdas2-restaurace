using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class LogController : Controller<Log>
    {
        public override List<Log> GetAll()
        {
            List<Log> result = new List<Log>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_log";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            result.Add(new Log
                            {
                                TableName = rdr.GetString(0),
                                AffectedId = rdr.GetInt32(1),
                                ActionType = rdr.GetString(2),
                                Time = rdr.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
