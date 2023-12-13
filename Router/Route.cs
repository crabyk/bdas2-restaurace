using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Router
{
    public class Route
    {
        public string Name { get; set; }
        public BindableBase ViewModel { get; set; }

        public virtual BindableBase NewViewModel()
        {
            ViewModel = new BindableBase();
            return ViewModel;
        }
    }

    public class Route<T> : Route
        where T : BindableBase, new()
    {
        public Route(string name)
        {
            Name = name;
        }

        public new T ViewModel
        {
            get => base.ViewModel as T;
            set => base.ViewModel = value;
        }

        public override BindableBase NewViewModel()
        {
            ViewModel = new T();
            return ViewModel;
        } 
	}

}
