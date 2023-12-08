using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.Router;

namespace BDAS2_Restaurace.ViewModel
{
    public class RegisterViewModel : ViewModelBase<User, UserController>
    {
        public RegisterViewModel() : base(new UserController())
        {

        }

        protected override void CreateMethod(object obj)
        {
            OnWindowClose();
            // base.CreateMethod(obj);
            // TODO vratit se zpet na domovskou stranku
        }
    }
}
