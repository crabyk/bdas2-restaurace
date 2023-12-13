using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class PartEmployeeViewModel : ViewModelBase<PartEmployee, PartEmployeeController>
    {
        private ObservableCollection<Employee> employees;
        private Employee selectedEmployee;

        private ObservableCollection<Address> addresses;
        private ObservableCollection<JobPosition> positions;

        private ObservableCollection<WorkShift> employeeShifts = new ObservableCollection<WorkShift>();
        private WorkShift selectedShift;
        private WorkShift newShift;

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ObservableCollection<Address> Addresses
        {
            get { return addresses; }
            set
            {
                addresses = value;
                OnPropertyChanged(nameof(Addresses));
            }
        }

        public ObservableCollection<JobPosition> Positions
        {
            get { return positions; }
            set
            {
                positions = value;
                OnPropertyChanged(nameof(Positions));
            }
        }

        public WorkShift SelectedShift
        {
            get { return selectedShift; }
            set
            {
                selectedShift = value;
                OnPropertyChanged(nameof(SelectedShift));
            }
        }

        public WorkShift NewShift
        {
            get { return newShift; }
            set
            {
                newShift = value;
                OnPropertyChanged(nameof(NewShift));
            }
        }

        public ObservableCollection<WorkShift> EmployeeShifts
        {
            get { return employeeShifts; }
            set
            {
                employeeShifts = value;
                OnPropertyChanged(nameof(EmployeeShifts));
            }
        }

        public ICommand RemoveShift { get; set; }
        public ICommand AddShift { get; set; }

        public ICommand NoManager { get; set; }

        public PartEmployeeViewModel() : base(new PartEmployeeController())
        {
            AddShift = new RelayCommand(AddShiftMethod, CanAddShiftMethod);
            RemoveShift = new RelayCommand(RemoveShiftMethod, CanRemoveShiftMethod);
            NoManager = new RelayCommand(NoManagerMethod, CanNoManagerMethod);
        }

        private bool CanNoManagerMethod(object arg)
        {
            return true;
        }

        private void NoManagerMethod(object obj)
        {
            SelectedItem.ManagerId = null;
        }

        private bool CanAddShiftMethod(object obj)
        {
            return true;
        }

        private void AddShiftMethod(object obj)
        {
            SelectedItem.AddShift(NewShift);
        }

        private bool CanRemoveShiftMethod(object obj)
        {
            return true;
        }

        private void RemoveShiftMethod(object obj)
        {
            SelectedItem.RemoveShift(SelectedShift);
        }


        private async void LoadEmployees()
        {
            List<FullEmployee> fullEmps = await Task.Run(() => new FullEmployeeController().GetAll());
            List<PartEmployee> partEmps = await Task.Run(() => new PartEmployeeController().GetAll());
            List<Employee> emps = new List<Employee>();

            emps.AddRange(fullEmps);
            emps.AddRange(partEmps);

            Employees = new ObservableCollection<Employee>(emps);
        }


        async void LoadPositions()
        {
            List<JobPosition> positions = await Task.Run(() => new JobPositionController().GetAll());
            Positions = new ObservableCollection<JobPosition>(positions);
        }

        async void LoadAddresses()
        {
            List<Address> addresses = await Task.Run(() => new AddressController().GetAll());
            Addresses = new ObservableCollection<Address>(addresses);
        }

        async void LoadShifts()
        {
            List<WorkShift> shifts = await Task.Run(() => new WorkShiftController().GetAll());
            EmployeeShifts = new ObservableCollection<WorkShift>(shifts);
        }

        protected override void Load()
        {
            LoadAddresses();
            LoadPositions();
            LoadShifts();
            LoadEmployees();
            base.Load();
        }

        protected override bool IsMatchingFilter(PartEmployee item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return
                item.FirstName.ToLower().Contains(filterTextLower) ||
                item.LastName.ToLower().Contains(filterTextLower) ||
                item.JobPosition.Name.ToLower().Contains(filterTextLower) ||
                item.EmploymentType.ToLower().Contains(filterTextLower);
        }
    }
}
