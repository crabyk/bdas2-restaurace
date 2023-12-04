using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Controller
{
	public abstract class Controller<T>
	{
		virtual public T? Add(T item)
		{
			throw new NotImplementedException();
		}

        virtual public int Delete(string id)
		{
			throw new NotImplementedException();
		}

        virtual public T? Get(string id)
		{
			throw new NotImplementedException();
		}

		virtual public List<T> GetAll()
		{
			throw new NotImplementedException();
		}

        virtual public T? Update(T item)
		{
			throw new NotImplementedException();
		}
	}
}
