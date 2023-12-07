using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;

namespace BDAS2_Restaurace.ViewModel
{
    public class RegisterViewModel : ViewModelBase<User, UserController>
    {
        public RegisterViewModel() : base(new UserController())
        {
        }

        protected override void CreateMethod(object obj)
        {
            base.CreateMethod(obj);
            // TODO vratit se zpet na domovskou stranku
        }
    }
}
