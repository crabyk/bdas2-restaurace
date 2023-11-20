using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.DB;
using BDAS2_Restaurace.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BDAS2_Restaurace
{
    /// <summary>
    /// Interakční logika pro MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        OracleConnection? conn = null;
        List<Food> foodList;
        List<Food> drinkList;
        bool foodSelected = true;
        bool drinkSelected = false;

        public MenuWindow()
        {
            InitializeComponent();

            foodList = new List<Food>();
            //GetFood();

            // Food newFood = new Food(0, "Nějaká mnamka", 120.0, 105.0, "Nejaky recept :D");
            
            // FoodController.Add(newFood); // Test vkladani
			foodList = FoodController.GetAll();


			var width = menuListView.Width - SystemParameters.VerticalScrollBarWidth;
            var columns = (menuListView.View as GridView).Columns;

            foreach (var column in columns)
            {
                column.Width = width / columns.Count;
            }
            menuListView.ItemsSource = foodList;
        }

        private void GetFood()
        {
            if (conn == null)
                conn = Database.Connect();

            using (conn)
            {
                conn.Open();
                string sql = "select nazev, cena, hmotnost, recept from polozky p join jidla j on p.id_polozka = j.id_polozka";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            // var food = new Food(0, rdr.GetString(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetString(3));
                            // foodList.Add(food);
                        }
                    }
                }
            }
        }

        private void GetDrinks()
        {
            /*
            if (conn == null)
                conn = Database.Connect();

            using (conn)
            {
                conn.Open();
                string sql = "select nazev, cena, objem from polozky p join napoje n on p.id_polozka = n.id_polozka";
                using (OracleCommand comm = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader rdr = comm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var drink = new Drink(rdr.GetString(0), rdr.GetInt32(1), rdr.GetInt32(2));
                            drinkList.Add(drink);
                        }
                    }
                }
            }
            */
            // Pro vyzkouseni odkomentovat

		}

		private void Food_Button_Click(object sender, RoutedEventArgs e)
		{
            foodSelected = true;
			drinkSelected = false;
		}

		private void Drink_Button_Click(object sender, RoutedEventArgs e)
		{
			foodSelected = false;
			drinkSelected = true;
		}
	}
}
