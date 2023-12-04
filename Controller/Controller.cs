using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Controller
{
	public abstract class Controller<T>
	{
		public static T? Add(T item)
		{
			throw new NotImplementedException();
		}

		public static int Delete(string id)
		{
			throw new NotImplementedException();
		}

		public static T? Get(string id)
		{
			throw new NotImplementedException();
		}

		public static List<T> GetAll()
		{
			throw new NotImplementedException();
		}

		public static T? Update(T item, string id)
		{
			throw new NotImplementedException();
		}
	}
}
