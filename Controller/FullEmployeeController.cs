using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Controller
{
    public class FullEmployeeController : Controller<FullEmployee>
    {

        public override FullEmployee? Add(FullEmployee item)
        {
            FullEmployee? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_uvazek";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;
                    comm.Parameters.Add("p_pozice_id", OracleDbType.Decimal).Value = item.JobPosition.ID;
                    comm.Parameters.Add("p_plat", OracleDbType.Int32).Value = item.MonthRate;
                    comm.Parameters.Add("p_id_zamestnanec", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_zamestnanec"].Value).Value;
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
                    comm.CommandText = "smazat_zamestnance";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_zamestnanec", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override FullEmployee? Get(string id)
        {
            FullEmployee? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_uvazek";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_zamestnanec", id);

                    OracleParameter firstName = new OracleParameter("p_jmeno", OracleDbType.Varchar2, 20, null, ParameterDirection.Output);
                    comm.Parameters.Add(firstName);
                    OracleParameter lastName = new OracleParameter("p_prijmeni", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    comm.Parameters.Add(lastName);
                    OracleParameter addressId = new OracleParameter("p_adresa_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(addressId);
                    OracleParameter positionId = new OracleParameter("p_pozice_id", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(positionId);
                    OracleParameter employmentType = new OracleParameter("p_typ_uvazku", OracleDbType.Varchar2, 11, null, ParameterDirection.Output);
                    comm.Parameters.Add(employmentType);
                    OracleParameter monthRate = new OracleParameter("p_plat", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(monthRate);

                    comm.ExecuteNonQuery();

                    var address = new AddressController().Get(addressId.Value.ToString());

                    result = new FullEmployee()
                    {
                        ID = int.Parse(id),
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        EmploymentType = employmentType.Value.ToString(),
                        MonthRate = float.Parse(monthRate.Value.ToString()),
                        Address = address
                    };
                }
            }

            return result;
        }

        public override List<FullEmployee> GetAll()
        {
            List<FullEmployee> result = new List<FullEmployee>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_uvazky";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Address address = new AddressController().Get(rdr.GetString(5));
                            result.Add(new FullEmployee
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),   
                                EmploymentType = rdr.GetString(3),
                                MonthRate = rdr.GetFloat(4),
                                Address = address
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override FullEmployee? Update(FullEmployee item)
        {
            FullEmployee? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_uvazek";
                    comm.CommandType = CommandType.StoredProcedure;


                    comm.Parameters.Add("p_id_zamestnanec", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;
                    comm.Parameters.Add("p_pozice_id", OracleDbType.Decimal).Value = item.JobPosition.ID;
                    comm.Parameters.Add("p_plat", OracleDbType.Int32).Value = item.MonthRate;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

    }
}
