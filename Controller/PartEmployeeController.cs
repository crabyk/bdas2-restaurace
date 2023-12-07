using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Media.Imaging;

namespace BDAS2_Restaurace.Controller
{
    public class PartEmployeeController : Controller<PartEmployee>
    {

        public override PartEmployee? Add(PartEmployee item)
        {
            PartEmployee? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                decimal newId;

                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "vlozit_brigadu";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;
                    comm.Parameters.Add("p_pozice_id", OracleDbType.Decimal).Value = item.JobPosition.ID;
                    comm.Parameters.Add("p_sazba", OracleDbType.Int32).Value = item.HourRate;
                    comm.Parameters.Add("p_id_zamestnanec", OracleDbType.Decimal, ParameterDirection.Output);

                    comm.ExecuteNonQuery();

                    newId = ((OracleDecimal)comm.Parameters["p_id_zamestnanec"].Value).Value;
                    result = item;
                    result.ID = Convert.ToInt32(newId);

                    foreach (WorkShift shift in item.Shifts)
                    {
                        new EmployeeShiftController().Add(shift, item);
                    }
                }

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

        public override PartEmployee? Get(string id)
        {
            PartEmployee? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_brigadu";
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
                    OracleParameter hourRate = new OracleParameter("p_sazba", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(hourRate);

                    comm.ExecuteNonQuery();

                    Address address = new AddressController().Get(addressId.Value.ToString());
                    JobPosition position = new JobPositionController().Get(positionId.Value.ToString());
                    List<WorkShift> shifts = new EmployeeShiftController().GetAll(id);

                    result = new PartEmployee()
                    {
                        ID = int.Parse(id),
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        EmploymentType = employmentType.Value.ToString(),
                        HourRate = float.Parse(hourRate.Value.ToString()),
                        Address = address,
                        JobPosition = position,
                        Shifts = new ObservableCollection<WorkShift>(shifts)
                    };
                }
            }

            return result;
        }

        public override List<PartEmployee> GetAll()
        {
            List<PartEmployee> result = new List<PartEmployee>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = "ziskat_brigady";
                    comm.CommandType = CommandType.StoredProcedure;

                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Address address = new AddressController().Get(rdr.GetString(5));
                            JobPosition position = new JobPositionController().Get(rdr.GetString(6));
                            List<WorkShift> shifts = new EmployeeShiftController().GetAll(rdr.GetInt32(0).ToString());

                            result.Add(new PartEmployee
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                EmploymentType = rdr.GetString(3),
                                HourRate = rdr.GetFloat(4),
                                Address = address,
                                JobPosition = position,
                                Shifts = new ObservableCollection<WorkShift>(shifts)
                            });
                        }
                    }
                }
            }

            return result;
        }

        public override PartEmployee? Update(PartEmployee item)
        {
            PartEmployee? result = null;

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();

                using (OracleCommand comm = conn.CreateCommand())
                {

                    List<WorkShift> shifts = new EmployeeShiftController().GetAll(item.ID.ToString());
                    List<WorkShift> newShifts = new List<WorkShift>(item.Shifts);

                    List<WorkShift> shiftsAdd = newShifts.Where(s1 => !shifts.Any(s2 => s2.ID == s1.ID)).ToList();
                    List<WorkShift> shiftsRemove = shifts.Where(s1 => !newShifts.Any(s2 => s2.ID == s1.ID)).ToList();

                    shiftsAdd.ForEach(s => new EmployeeShiftController().Add(s, item));
                    shiftsRemove.ForEach(s => new EmployeeShiftController().Delete(s, item));


                    comm.CommandText = "upravit_brigadu";
                    comm.CommandType = CommandType.StoredProcedure;


                    comm.Parameters.Add("p_id_zamestnanec", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;
                    comm.Parameters.Add("p_pozice_id", OracleDbType.Decimal).Value = item.JobPosition.ID;
                    comm.Parameters.Add("p_sazba", OracleDbType.Int32).Value = item.HourRate;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

    }
}
