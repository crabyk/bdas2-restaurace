using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BDAS2_Restaurace.Windows
{
    /// <summary>
    /// Interakční logika pro CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow(Customer customer)
        {
            DataContext = new UserCustomerViewModel(customer);
            InitializeComponent();
        }
    }
}
