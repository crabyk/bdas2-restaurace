using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace BDAS2_Restaurace.Controller
{
    public class UserController : Controller<User>
    {

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the plain text password to a byte array
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash value of the byte array
                byte[] hash = sha256.ComputeHash(bytes);

                // Convert the hash value to a base64-encoded string
                string hashedPassword = Convert.ToBase64String(hash);

                return hashedPassword;
            }
        }

        public override int Delete(string id)
        {
            int result = 0;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "smazat_uzivatele";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_uzivatel", id);

                    result = comm.ExecuteNonQuery();
                }
            }

            return result;
        }

        public override User? Get(string id)
        {
            User? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_uzivatele";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_uzivatel", id);

                    OracleParameter login = new OracleParameter("p_login", OracleDbType.Varchar2, 100, null, ParameterDirection.Output);
                    comm.Parameters.Add(login);
                    OracleParameter hash = new OracleParameter("p_hash", OracleDbType.Varchar2, 255, null, ParameterDirection.Output);
                    comm.Parameters.Add(hash);
                    OracleParameter firstName = new OracleParameter("p_jmeno", OracleDbType.Varchar2, 20, null, ParameterDirection.Output);
                    comm.Parameters.Add(firstName);
                    OracleParameter lastName = new OracleParameter("p_prijmeni", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    comm.Parameters.Add(lastName);
                    OracleParameter roleId = new OracleParameter("p_id_role", OracleDbType.Decimal, ParameterDirection.Output);
                    comm.Parameters.Add(roleId);

                    try
                    {
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                    var role = new RoleController().Get(roleId.Value.ToString());

                    result = new User()
                    {
                        ID = int.Parse(id),
                        Login = login.Value.ToString(),
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        Role = role
                    };
                }
            }

            return result;
        }

        public override List<User> GetAll()
        {
            List<User> result = new List<User>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_vsechny_uzivatele";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var role = new RoleController().Get(rdr.GetInt32(5).ToString());

                            result.Add(new User
                            {
                                ID = rdr.GetInt32(0),
                                Login = rdr.GetString(1),
                                FirstName = rdr.GetString(3),
                                LastName = rdr.GetString(4),
                                Role = role
                            });
                        }
                    }
                }
            }

            return result;
        }

        public User? Update(User item, string password)
        {
            User? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "upravit_uzivatele";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_id_uzivatel", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_login", OracleDbType.Varchar2).Value = item.Login;
                    comm.Parameters.Add("p_hash", OracleDbType.Varchar2).Value = HashPassword(password);
                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_id_role", OracleDbType.Decimal).Value = item.Role.ID;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

        public User? Register(User item, string password)
        {
            User? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_uzivatele";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_login", OracleDbType.Varchar2).Value = item.Login;
                    comm.Parameters.Add("p_hash", OracleDbType.Varchar2).Value = HashPassword(password);
                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_id_role", OracleDbType.Decimal).Value = item.Role.ID;
                    comm.Parameters.Add("p_id_uzivatel", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_uzivatel"].Value).Value;
                }

                result = item;
                result.ID = Convert.ToInt32(newId);
            }

            return result;
        }

        public User? Login(string username, string password)
        {
            User? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "prihlasit_uzivatele";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_login", username);
                    comm.Parameters.Add("p_hash", HashPassword(password));

                    OracleParameter userId = new OracleParameter("p_id_uzivatel", OracleDbType.Decimal, ParameterDirection.Output);
                    comm.Parameters.Add(userId);
                    OracleParameter firstName = new OracleParameter("p_jmeno", OracleDbType.Varchar2, 20, null, ParameterDirection.Output);
                    comm.Parameters.Add(firstName);
                    OracleParameter lastName = new OracleParameter("p_prijmeni", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    comm.Parameters.Add(lastName);
                    OracleParameter roleId = new OracleParameter("p_id_role", OracleDbType.Decimal, ParameterDirection.Output);
                    comm.Parameters.Add(roleId);

                    comm.ExecuteNonQuery();

                    var role = new RoleController().Get(roleId.Value.ToString());

                    result = new User()
                    {
                        Login = username,
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        Role = role
                    };
                }
            }

            return result;
        }
    }
}
