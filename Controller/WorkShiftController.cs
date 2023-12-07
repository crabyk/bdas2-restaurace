using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class WorkShiftController : Controller<WorkShift>
    {
        public override WorkShift? Add(WorkShift item)
        {
            WorkShift? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_smenu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_cas_zacatek", OracleDbType.Date).Value = item.Begin;
                    comm.Parameters.Add("p_cas_konec", OracleDbType.Date).Value = item.End;
                    comm.Parameters.Add("p_id_smena", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_smena"].Value).Value;
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
                    comm.CommandText = "smazat_smenu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_smena", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override WorkShift? Get(string id)
        {
            WorkShift? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_smenu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_smena", id);

                    OracleParameter beginDate = new OracleParameter("p_cas_zacatek", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(beginDate);
                    OracleParameter endDate = new OracleParameter("p_cas_konec", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(endDate);



                    comm.ExecuteNonQuery();

                    result = new WorkShift()
                    {
                        ID = int.Parse(id),
                        Begin = ((OracleDate)beginDate.Value).Value,
                        End = ((OracleDate)endDate.Value).Value
                    };
                }
            }

            return result;
        }

        public override List<WorkShift> GetAll()
        {
            List<WorkShift> result = new List<WorkShift>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_smeny";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new WorkShift
                            {
                                ID = rdr.GetInt32(0),
                                Begin = rdr.GetDateTime(1),
                                End = rdr.GetDateTime(2)
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override WorkShift? Update(WorkShift item)
        {
            WorkShift? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_smenu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_smena", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_cas_zacatek", OracleDbType.Date).Value = item.Begin;
                    comm.Parameters.Add("p_cas_konec", OracleDbType.Date).Value = item.End;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
