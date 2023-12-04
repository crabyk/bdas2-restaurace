using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Controller
{
	public interface ControllerInterface<T>
	{
		T? Add(T item);
		int Delete(string id);
		T? Get(string id);
		List<T> GetAll();
		T? Update(T item, string id);
	}
}
