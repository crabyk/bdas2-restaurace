using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    public class EmployeeShiftController
    {
        public WorkShift Add(WorkShift shift, Employee employee)
        {
            WorkShift? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "insert into zam_smeny (zamestnanec_id, smena_id) VALUES (:zamestnanecId, :smenaId)";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Clear();
                    comm.Parameters.Add(":zamestnanecId", OracleDbType.Int32).Value = employee.ID;
                    comm.Parameters.Add(":smenaId", OracleDbType.Int32).Value = shift.ID;
                    comm.ExecuteNonQuery();
                }

                result = shift;
            }

            return result;
        }

        public WorkShift Delete(WorkShift shift, Employee employee)
        {
            WorkShift? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "delete from zam_smeny where zamestnanec_id = :zamestnanecId and smena_id = :smenaId";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Clear();
                    comm.Parameters.Add(":zamestnanecId", OracleDbType.Int32).Value = employee.ID;
                    comm.Parameters.Add(":smenaId", OracleDbType.Int32).Value = shift.ID;
                    comm.ExecuteNonQuery();
                }

                result = shift;
            }

            return result;
        }

        public List<WorkShift> GetAll(string employeeId)
        {
            List<WorkShift> result = new List<WorkShift>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = string.Empty;

                sql = "select s.id_smena, cas_zacatek, cas_konec from smeny s " +
                        "join zam_smeny zs on s.id_smena = zs.smena_id " +
                        "where zamestnanec_id = :id";

                using (OracleCommand comm = new OracleCommand(sql, conn))
                {

                    comm.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = employeeId;

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(
                                new WorkShift
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
    }
}
