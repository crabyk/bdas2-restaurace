using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class RoleController : Controller<Role>
    {
        public override Role? Add(Role item)
        {
            Role? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_roli";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;
                    comm.Parameters.Add("p_id_role", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_role"].Value).Value;
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
                    comm.CommandText = "smazat_roli";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_role", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Role? Get(string id)
        {
            Role? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_roli";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_role", id);

                    OracleParameter name = new OracleParameter("p_nazev", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    comm.Parameters.Add(name);

                    comm.ExecuteNonQuery();

                    result = new Role()
                    {
                        ID = int.Parse(id),
                        Name = name.Value.ToString()
                    };
                }
            }

            return result;
        }

        public override List<Role> GetAll()
        {
            List<Role> result = new List<Role>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_role";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new Role
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

        public override Role? Update(Role item)
        {
            Role? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_roli";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_role", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
