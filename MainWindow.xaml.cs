using System.Windows;

namespace BDAS2_Restaurace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
		private void FoodViewList_Loaded(object sender, RoutedEventArgs e)
		{
			BDAS2_Restaurace.ViewModel.FoodViewModel foodViewModelObject = new BDAS2_Restaurace.ViewModel.FoodViewModel();
			foodViewModelObject.LoadFood();

			FoodViewList.DataContext = foodViewModelObject;
		}
        */


		/*
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();          
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
        }
        */
	}
}
