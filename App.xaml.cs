using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BDAS2_Restaurace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Predani delegata k zpracovani neosetrenych vyjimek (kdekoli v aplikaci)
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Zpracovani vyjimky ve forme okna s chybovou hlaskou
            MessageBox.Show($"Došlo k chybě: {e.Exception.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);

            // Nastaveni, aby se vyjimka dal nesirila
            e.Handled = true;
        }
    }
}
