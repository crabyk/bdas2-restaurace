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
        private ObservableCollection<Address> addresses;
        private ObservableCollection<JobPosition> positions;

        private ObservableCollection<WorkShift> employeeShifts = new ObservableCollection<WorkShift>();
        private WorkShift selectedShift;
        private WorkShift newShift;

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

        public PartEmployeeViewModel() : base(new PartEmployeeController())
        {
            AddShift = new RelayCommand(AddShiftMethod, CanAddShiftMethod);
            RemoveShift = new RelayCommand(RemoveShiftMethod, CanRemoveShiftMethod);
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
            base.Load();
        }
    }
}
