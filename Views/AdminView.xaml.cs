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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDAS2_Restaurace.Views
{
	/// <summary>
	/// Interakční logika pro AdminView.xaml
	/// </summary>
	public partial class AdminView : UserControl
	{
		public AdminView()
		{
			InitializeComponent();

			BindableBase viewModel = Content.DataContext as BindableBase;

			if (viewModel is DrinkViewModel)
			{
				Headline.Content = "Drinky";
			}
		}
	}
}
