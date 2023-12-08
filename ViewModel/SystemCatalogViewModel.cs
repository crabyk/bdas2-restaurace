using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.ViewModel
{
    public class SystemCatalogViewModel : ViewModelBase<SystemCatalog, SystemCatalogController>
    {
        public SystemCatalogViewModel() : base (new SystemCatalogController())
        {

        }
    }
}
