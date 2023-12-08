using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class SystemCatalogController : Controller<SystemCatalog>
    {
        public override List<SystemCatalog> GetAll()
        {
            List<SystemCatalog> result = new List<SystemCatalog>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_systemovy_katalog";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new SystemCatalog
                            {
                                ID = rdr.GetInt32(0),
                                ObjectName = rdr.GetString(1),
                                ObjectType = rdr.GetString(2),
                                Created = rdr.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
