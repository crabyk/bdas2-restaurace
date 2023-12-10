using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace BDAS2_Restaurace.Errors
{
    public enum ErrorType
    {
        Create,
        Update,
        Get,
        GetAll,
        Delete
    }
    public class ErrorHandler
    {

        public static void OpenDialog(string message, string caption)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;

            MessageBox.Show(message, caption, button, icon, MessageBoxResult.Yes);
        }

        public static void OpenDialog(ErrorType type)
        {
            string caption = "Chyba";
            string message = "Nastala neznámá chyba";

            switch (type)
            {
                case ErrorType.Create:
                    caption = "Chyba při vytváření dat";
                    message = "Zkontrolujte zda jsou zadané údaje správné";
                    break;
                case ErrorType.Update:
                    caption = "Chyba při auktualizaci dat";
                    message = "Zkontrolujte zda jsou zadané údaje správné a je vybraná položka k aktualizaci";
                    break;
                case ErrorType.Get:
                    caption = "Cyhba při načítaní dat";
                    message = "Zkontrolujte zda je nastaven správně parametr pro získání dat";
                    break;
                case ErrorType.GetAll:
                    caption = "Chyba při načítání kolekce dat";
                    message = "Neočekávaná chyba při získávání dat z databáze";
                    break;
                case ErrorType.Delete:
                    caption = "Chyba při mazání dat";
                    message = "Zkontrolujte jestli je vybraná položka k mazání";
                    break;
            }

            OpenDialog(message, caption);

        }
    }
}
