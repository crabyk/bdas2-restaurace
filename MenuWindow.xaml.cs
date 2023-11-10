using BDAS2_Restaurace.db;
using BDAS2_Restaurace.models;
using Oracle.ManagedDataAccess.Client;
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
        List<Drink> drinkList;

        public MenuWindow()
        {
            InitializeComponent();

            foodList = new List<Food>();
            GetFood();

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
                            var food = new Food(rdr.GetString(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetString(3));
                            foodList.Add(food);
                        }
                    }
                }
            }
        }

        private void GetDrinks()
        {
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
        }
    }
}
