﻿using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

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
                    comm.Parameters.Add("p_id_nadrizeny", OracleDbType.Int32).Value = item.ManagerId;
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
                    comm.CommandText = "smazat_uvazek";
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
                    OracleParameter managerId = new OracleParameter("p_id_nadrizeny", OracleDbType.Int32, ParameterDirection.Output);
                    comm.Parameters.Add(managerId);

                    comm.ExecuteNonQuery();


                    Address address = new AddressController().Get(addressId.Value.ToString());
                    JobPosition position = new JobPositionController().Get(positionId.Value.ToString());
                    List<WorkShift> shifts = new EmployeeShiftController().GetAll(id);

                    result = new FullEmployee()
                    {
                        ID = int.Parse(id),
                        FirstName = firstName.Value.ToString(),
                        LastName = lastName.Value.ToString(),
                        EmploymentType = employmentType.Value.ToString(),
                        MonthRate = float.Parse(monthRate.Value.ToString()),
                        Address = address,
                        JobPosition = position,
                        Shifts = new ObservableCollection<WorkShift>(shifts),
                        ManagerId = (managerId.Value == DBNull.Value) ? null : int.Parse(managerId.Value.ToString())
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
                            JobPosition position = new JobPositionController().Get(rdr.GetString(6));
                            List<WorkShift> shifts = new EmployeeShiftController().GetAll(rdr.GetInt32(0).ToString());

                            int? managerId = null;
                            if (!rdr.IsDBNull(7))
                                managerId = rdr.GetInt32(7);

                            result.Add(new FullEmployee
                            {
                                ID = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                EmploymentType = rdr.GetString(3),
                                MonthRate = rdr.GetFloat(4),
                                Address = address,
                                JobPosition = position,
                                Shifts = new ObservableCollection<WorkShift>(shifts),
                                ManagerId = managerId
                            });
                        }
                    }
                }
            }

            return result;
        }

        public List<Employee> GetTree()
        {
            List<Employee> result = new List<Employee>();

            using (OracleConnection conn = Database.Connect())
            {
                conn.Open();
                using (OracleCommand comm = conn.CreateCommand())
                {

                    comm.CommandText = "ziskat_zamestnance_hr";
                    comm.CommandType = CommandType.StoredProcedure;


                    comm.Parameters.Add("p_kurzor", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            JobPosition position = new JobPositionController().Get(rdr.GetString(4));
                            int? managerId = null;
                            if (!rdr.IsDBNull(1))
                                managerId = rdr.GetInt32(1);

                            result.Add(new FullEmployee
                            {
                                ID = rdr.GetInt32(0),
                                ManagerId = rdr.IsDBNull(1) ? (int?)null : rdr.GetInt32(1),
                                FirstName = rdr.GetString(2),
                                LastName = rdr.GetString(3),
                                JobPosition = position
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
                    List<WorkShift> shifts = new EmployeeShiftController().GetAll(item.ID.ToString());
                    List<WorkShift> newShifts = new List<WorkShift>(item.Shifts);

                    List<WorkShift> shiftsAdd = newShifts.Where(s1 => !shifts.Any(s2 => s2.ID == s1.ID)).ToList();
                    List<WorkShift> shiftsRemove = shifts.Where(s1 => !newShifts.Any(s2 => s2.ID == s1.ID)).ToList();

                    shiftsAdd.ForEach(s => new EmployeeShiftController().Add(s, item));
                    shiftsRemove.ForEach(s => new EmployeeShiftController().Delete(s, item));

                    comm.CommandText = "upravit_uvazek";
                    comm.CommandType = CommandType.StoredProcedure;


                    comm.Parameters.Add("p_id_zamestnanec", OracleDbType.Decimal).Value = item.ID;
                    comm.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = item.FirstName;
                    comm.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = item.LastName;
                    comm.Parameters.Add("p_adresa_id", OracleDbType.Decimal).Value = item.Address.ID;
                    comm.Parameters.Add("p_pozice_id", OracleDbType.Decimal).Value = item.JobPosition.ID;
                    comm.Parameters.Add("p_plat", OracleDbType.Int32).Value = item.MonthRate;
                    comm.Parameters.Add("p_id_nadrizeny", OracleDbType.Int32).Value = item.ManagerId;

                    comm.ExecuteNonQuery();
                }

                result = item;
            }

            return result;
        }

    }
}
