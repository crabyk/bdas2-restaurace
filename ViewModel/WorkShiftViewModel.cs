using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class WorkShiftViewModel : ViewModelBase<WorkShift, WorkShiftController>
    {
        public WorkShiftViewModel() : base(new WorkShiftController())
        {
        }
    }
}
