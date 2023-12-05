using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Controller
{
    public class PaymentTypeController : Controller<PaymentType>
    {
        public override PaymentType? Add(PaymentType item)
        {
            PaymentType? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_typ_platby";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = item.Name;
                    comm.Parameters.Add("p_id_typ_platby", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_typ_platby"].Value).Value;
                }

                result = item;
                result.ID = Convert.ToInt32(newId);
            }

            return result;
        }

        public override PaymentType? Get(string id)
        {
            PaymentType? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_typ_platby";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_typ_platby", id);

                    OracleParameter name = new OracleParameter("p_nazev", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(name);

                    comm.ExecuteNonQuery();

                    result = new PaymentType()
                    {
                        ID = int.Parse(id),
                        Name = name.Value.ToString()
                    };
                }
            }

            return result;
        }

        public override List<PaymentType> GetAll()
        {
            List<PaymentType> result = new List<PaymentType>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select id_typ_platby, nazev from typy_plateb";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result.Add(new PaymentType
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
    }
}
