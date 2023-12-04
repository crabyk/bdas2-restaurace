using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace BDAS2_Restaurace.Controller
{
    class AddressController : Controller<Address>
    {
        public override Address? Add(Address item)
        {
            Address? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_adresu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = item.StreetName;
                    comm.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = item.CityName;
                    comm.Parameters.Add("p_cislo_popisne", OracleDbType.Varchar2).Value = item.UnitNumber;
                    comm.Parameters.Add("p_psc", OracleDbType.Varchar2).Value = item.PostalCode;
                    comm.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = item.Country;
                    comm.Parameters.Add("p_id_adresa", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_adresa"].Value).Value;
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
                    comm.CommandText = "smazat_adresu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_adresa", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override Address? Get(string id)
        {
            Address? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_adresu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_adresa", id);

                    OracleParameter streetName = new OracleParameter("p_ulice", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(streetName);
                    OracleParameter cityName = new OracleParameter("p_mesto", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(cityName);
                    OracleParameter unitNumber = new OracleParameter("p_cislo_popisne", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(unitNumber);
                    OracleParameter postalCode = new OracleParameter("p_psc", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(postalCode);
                    OracleParameter country = new OracleParameter("p_stat", OracleDbType.Varchar2, ParameterDirection.Output);
                    comm.Parameters.Add(country);

                    // Asi nejaky problem s datovymi typi u procedury v DB nebo nevim :D
                    // Aspon se diky tomu da krasne otestovat ze program ted pada az kdyz se nacitaji Customers a ne hned pri spusteni
                    comm.ExecuteNonQuery();

                    result = new Address()
                    {
                        ID = int.Parse(id),
                        StreetName = streetName.Value.ToString(),
                        CityName = cityName.Value.ToString(),
                        UnitNumber = unitNumber.Value.ToString(),
                        PostalCode = postalCode.Value.ToString(),
                        Country = country.Value.ToString()
                    };
                }
            }

            return result;
        }

        public override List<Address> GetAll()
        {
            List<Address> result = new List<Address>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                string sql = "select id_adresa, ulice, mesto, cislo_popisne, psc, stat from adresy";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            //result.Add(new Address(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5)));
                        }
                    }
                }

            }

            return result;
        }

        public override Address? Update(Address item)
        {
            Address? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_adresu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_adresa", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = item.StreetName;
                    comm.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = item.CityName;
                    comm.Parameters.Add("p_cislo_popisne", OracleDbType.Varchar2).Value = item.UnitNumber;
                    comm.Parameters.Add("p_psc", OracleDbType.Varchar2).Value = item.PostalCode;
                    comm.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = item.Country;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }
    }
}
