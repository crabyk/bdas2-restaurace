using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.controllers
{
	/// <summary>
	/// Interface pro zakladni komunikaci s databazi
	/// </summary>
	/// <typeparam name="T">Model/Objekt, ktery reflektuje konkretni databazi se kterou controller pracuje</typeparam>
	internal interface IController<T>
	{

		/// <summary>
		/// Vybere vsechna zaznamy z databazove tabulky
		/// </summary>
		/// <returns>Pole polozek</returns>
		static List<T> GetAll() => throw new NotImplementedException();

		/// <summary>
		/// Vybere konkretni zaznam z databazove tabulky
		/// </summary>
		/// <param name="id">Identifikator hledaneho zaznamu</param>
		/// <returns>Nalezeny zaznam pri uspechu, jinak null</returns>
		static T Get(string id) => throw new NotImplementedException();

		/// <summary>
		/// Vlozi novy zaznam do tabulky v databazi
		/// </summary>
		/// <param name="item">Novy zaznam</param>
		/// <returns>Vlozeny zaznam pri uspechu, jinak null</returns>
		static T Add(T item) => throw new NotImplementedException();

		/// <summary>
		/// Upravy zaznam databazove tabulky
		/// </summary>
		/// <param name="item">Upraveny zaznam</param>
		/// <param name="id">Identifikator upravovaneho zaznamu</param>
		/// <returns>Upraveny zaznam pri uspechu, jinak null</returns>
		static T Update(T item, string id) => throw new NotImplementedException();

		/// <summary>
		/// Odebere zaznam databazove tabulky
		/// </summary>
		/// <param name="id">Identifikator odebiraneho zaznamu</param>
		/// <returns>Pocet smazanych zaznamu</returns>
		static int Delete(string id) => throw new NotImplementedException();
	}
}
