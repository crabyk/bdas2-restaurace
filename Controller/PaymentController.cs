using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    class PaymentController : Controller<Payment>
    {

        public override Payment? Add(Payment item)
        {
            Payment? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_platbu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_suma", OracleDbType.Double).Value = item.Amount;
                    comm.Parameters.Add("p_datum", OracleDbType.Date).Value = item.Date;
                    comm.Parameters.Add("p_typ_platby_id", OracleDbType.Decimal).Value = item.Type.ID;
                    comm.Parameters.Add(":p_id_platba", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters[":p_id_platba"].Value).Value;
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
                    comm.CommandText = "smazat_platbu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_platba", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Payment? Get(string id)
        {
            Payment? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_platbu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_platba", id);

                    OracleParameter amount = new OracleParameter("p_suma", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(amount);
                    OracleParameter date = new OracleParameter("p_datum", OracleDbType.Date, ParameterDirection.Output);
                    comm.Parameters.Add(date);
                    OracleParameter typeId = new OracleParameter("p_typ_platby_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(typeId);

                    comm.ExecuteNonQuery();

                    var type = new PaymentTypeController().Get(typeId.Value.ToString());

                    result = new Payment()
                    {
                        ID = int.Parse(id),
                        Amount = double.Parse(amount.Value.ToString()),
                        Date = ((OracleDate)date.Value).Value,
                        Type = type
                    };
                }
            }

            return result;
        }

        public override List<Payment> GetAll()
        {
            List<Payment> result = new List<Payment>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select t.id_typ_platby, t.nazev, id_platba, suma, datum  from platby p join typy_plateb t on p.typ_platby_id = t.id_typ_platby";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            PaymentType paymentType = new PaymentType
                            {
                                ID = rdr.GetInt32(0),
                                Name = rdr.GetString(1)
                            };
                            result.Add(new Payment
                            {
                                ID = rdr.GetInt32(2),
                                Amount = rdr.GetDouble(3),
                                Date = rdr.GetDateTime(4),
                                Type = paymentType
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override Payment? Update(Payment item)
        {
            Payment? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_platbu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_platba", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_suma", OracleDbType.Int32).Value = item.Amount;
                    comm.Parameters.Add("p_datum", OracleDbType.Date).Value = item.Date;
                    comm.Parameters.Add("p_typ_platby_id", OracleDbType.Decimal).Value = item.Type.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
