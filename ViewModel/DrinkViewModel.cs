using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BDAS2_Restaurace.ViewModel
{
    public class DrinkViewModel : ViewModelBase<Drink, DrinkController>
    {

        public DrinkViewModel() : base(new DrinkController())
        {
        }

    }
}
