using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BDAS2_Restaurace.Model
{
	abstract public class Employee : Person
	{
		private JobPosition jobPosition;
		private ObservableCollection<WorkShift> shifts;
		private string employmentType;

		protected Employee()
		{
			jobPosition = new JobPosition();
			shifts = new ObservableCollection<WorkShift>();
		}

		public JobPosition JobPosition
		{
			get { return jobPosition; }
			set
			{
				jobPosition = value;
				OnPropertyChanged(nameof(JobPosition));
			}
		}

        public ObservableCollection<WorkShift> Shifts
        {
            get { return shifts; }
            set
            {
                shifts = value;
                OnPropertyChanged(nameof(Shifts));
            }
        }


        public string EmploymentType
        {
            get { return employmentType; }
            set
            {
                employmentType = value;
                OnPropertyChanged(nameof(EmploymentType));  
            }
        }

        public void AddShift(WorkShift shift)
        {
            Shifts.Add(shift);
            OnPropertyChanged(nameof(Shifts));
        }

        public void RemoveShift(WorkShift shift)
        {
            Shifts.Remove(shift);
            OnPropertyChanged(nameof(Shifts));
        }


        public override object Clone()
        {
            Employee employee = (Employee)MemberwiseClone();
			employee.JobPosition = (JobPosition)JobPosition.Clone();
            employee.Address = (Address)Address.Clone();

            employee.Shifts = new ObservableCollection<WorkShift>();
            foreach (var shift in Shifts)
                employee.AddShift((WorkShift)shift.Clone());

            return employee;
        }

    }
}
