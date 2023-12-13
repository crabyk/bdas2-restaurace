using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BDAS2_Restaurace.ViewModel
{
    public class EmployeeTreeViewModel : BindableBase
    {
        private string treeText;
        private Employee selectedItem;
        private ObservableCollection<Employee> items;

        public string TreeText
        {
            get { return treeText; }
            set
            {
                treeText = value;
                OnPropertyChanged(nameof(TreeText));    
            }
        }
        public Employee SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ObservableCollection<Employee> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }


        public EmployeeTreeViewModel()
        {
            List<Employee> items = new FullEmployeeController().GetTree();
            Items = new ObservableCollection<Employee>(items);
            SelectedItem = Items.First();
            TreeText = BuildTree(items);
        }

        public string BuildTree(List<Employee> emps)
        {
            var topLevel = emps.Where(e => e.ManagerId == null).ToList();
            return string.Join(Environment.NewLine, topLevel.Select(emp => PrintNode(emp, emps, 0)));
        }

        private string PrintNode(Employee emp, List<Employee> emps, int level)
        {
            string indentation = new string(' ', level * 4);
            string result = $"{indentation}{emp.FullName}";

            var subordinates = emps.Where(e => e.ManagerId == emp.ID).ToList();

            foreach (var subordinate in subordinates)
            {
                result += Environment.NewLine + PrintNode(subordinate, emps, level + 1);
            }

            return result;
        }
    }
}
