using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class JobPositionViewModel : ViewModelBase<JobPosition, JobPositionController>
    {
        public JobPositionViewModel() : base(new JobPositionController())
        {

        }

        protected override bool IsMatchingFilter(JobPosition item)
        {
            if (item == null)
                return false;

            string filterTextLower = FilterText.ToLower();

            return item.Name.ToLower().Contains(filterTextLower);
        }
    }
}
