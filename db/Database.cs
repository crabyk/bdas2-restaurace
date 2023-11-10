using Oracle.ManagedDataAccess.Client;

namespace BDAS2_Restaurace.db
{
    public static class Database
    {
        public static OracleConnection Connect()
        {
            string connectionString =
                "User Id=st67264;Password=Dy6ngKrXuS;Data Source=(DESCRIPTION=" +
                "(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))" +
                "(CONNECT_DATA=(SID=BDAS)));";
            return new OracleConnection(connectionString);
        }
    }
}
